
using System;
using System.Drawing;
using System.Windows.Forms;

namespace waitlinetest_frontandqueue
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.player = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.player)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "Work1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(92, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 48);
            this.button2.TabIndex = 1;
            this.button2.Text = "Work2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(92, 61);
            this.button11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(77, 48);
            this.button11.TabIndex = 7;
            this.button11.Text = "Stop2";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(10, 61);
            this.button12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(77, 48);
            this.button12.TabIndex = 6;
            this.button12.Text = "Stop1";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 15;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 5000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // player
            // 
            this.player.Enabled = true;
            this.player.Location = new System.Drawing.Point(184, 86);
            this.player.Name = "player";
            this.player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("player.OcxState")));
            this.player.Size = new System.Drawing.Size(29, 23);
            this.player.TabIndex = 22;
            this.player.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 122);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.player);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.player)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region SetUI Form
        private void setUIform()
        {
            //set Nowtime
            timer1.Enabled = true;
            timer1.Interval = 15;
            timer1.Start();

            GetTicketButton1 = new Button(); // take ticket
            GetTicketButton2 = new Button(); // take ticket 2
            ShowUItime = new Label();
            ShowUImsg = new Label();

            //set form2 background
            form2.Height = SystemInformation.WorkingArea.Height;
            form2.Width = SystemInformation.WorkingArea.Width;
            form2.Visible = true;
            form2.BackgroundImage = new Bitmap(@"..\..\UI_resource\B_background.jpg");
            form2.BackgroundImageLayout = ImageLayout.Stretch;

            //set form2 waiting button
            GetTicketButton1.BackgroundImage = new Bitmap(@"..\..\UI_resource\B_button.png");
            GetTicketButton1.BackgroundImageLayout = ImageLayout.Stretch;
            GetTicketButton1.BackColor = Color.Transparent;
            GetTicketButton1.ForeColor = Color.Transparent;
            GetTicketButton1.FlatStyle = FlatStyle.Flat;
            GetTicketButton1.FlatAppearance.BorderSize = 0;
            GetTicketButton1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            GetTicketButton1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            GetTicketButton1.Text = "take ticket";
            GetTicketButton1.ForeColor = Color.Black;
            GetTicketButton1.Font = new Font("微軟正黑體", 24, FontStyle.Bold);
            GetTicketButton1.Height = 450;
            GetTicketButton1.Width = 450;
            GetTicketButton1.Click += new EventHandler(GetTicketButton1_Click);

            //add GetTicketButton1 Button
            form2.AcceptButton = GetTicketButton1; //add takeTicket button
            form2.Controls.Add(GetTicketButton1);
            GetTicketButton1.Location = new Point(250, 200);

            //set form2 waiting button
            GetTicketButton2.BackgroundImage = new Bitmap(@"..\..\UI_resource\B_button.png");
            GetTicketButton2.BackgroundImageLayout = ImageLayout.Stretch;
            GetTicketButton2.BackColor = Color.Transparent;
            GetTicketButton2.ForeColor = Color.Transparent;
            GetTicketButton2.FlatStyle = FlatStyle.Flat;
            GetTicketButton2.FlatAppearance.BorderSize = 0;
            GetTicketButton2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            GetTicketButton2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            GetTicketButton2.Text = "take ticket";
            GetTicketButton2.ForeColor = Color.Black;
            GetTicketButton2.Font = new Font("微軟正黑體", 24, FontStyle.Bold);
            GetTicketButton2.Height = 450;
            GetTicketButton2.Width = 450;
            GetTicketButton2.Click += new EventHandler(GetTicketButton2_Click);

            //add GetTicketButton2 Button
            form2.AcceptButton = GetTicketButton2; //add takeTicket button
            form2.Controls.Add(GetTicketButton2);
            GetTicketButton2.Location = new Point(250, 800);

            //set ShowUI label
            ShowUItime.Name = "ShowUItime";
            ShowUItime.Left = 200;
            ShowUItime.Top = 1800;
            ShowUItime.Width = 700;
            ShowUItime.Height = 100;
            ShowUItime.BackColor = Color.Transparent;
            ShowUItime.ForeColor = Color.Black;

            ShowUImsg.Name = "ShowUImsg";
            ShowUImsg.Left = form2.Width;
            ShowUImsg.Top = 30;
            ShowUImsg.Width = 2700;
            ShowUImsg.Height = 42;
            ShowUImsg.BackColor = Color.Transparent;
            ShowUImsg.ForeColor = Color.Black;
            ShowUImsg.Text = text;

            ShowUItime.Font = new Font("微軟正黑體", 24, FontStyle.Bold);
            ShowUImsg.Font = new Font("微軟正黑體", 24, FontStyle.Bold);
            form2.Controls.Add(ShowUItime);
            form2.Controls.Add(ShowUImsg);
        }
        private void GetTicketButton1_Click(object sender, EventArgs e)
        {
            AddWaiting(0);
        }
        private void GetTicketButton2_Click(object sender, EventArgs e)
        {
            AddWaiting(1);
        }
        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private  AxWMPLib.AxWindowsMediaPlayer player;

        private Button GetTicketButton1; // take ticket
        private Button GetTicketButton2;  // take ticket 2
        private Label ShowUItime;
        private Label ShowUImsg;
    }
}

