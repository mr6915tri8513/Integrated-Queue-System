
namespace waitlinetest_frontandqueue
{
    partial class DataFormatEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DataNameLabel = new System.Windows.Forms.Label();
            this.DataNameTextBox = new System.Windows.Forms.TextBox();
            this.DataTypeLabel = new System.Windows.Forms.Label();
            this.DataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.DataSelectItemLabel = new System.Windows.Forms.Label();
            this.DataSelectItemListBox = new System.Windows.Forms.ListBox();
            this.DataSelectItemEditButton = new System.Windows.Forms.Button();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.RequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DataNameLabel
            // 
            this.DataNameLabel.AutoSize = true;
            this.DataNameLabel.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataNameLabel.Location = new System.Drawing.Point(17, 9);
            this.DataNameLabel.Name = "DataNameLabel";
            this.DataNameLabel.Size = new System.Drawing.Size(163, 30);
            this.DataNameLabel.TabIndex = 0;
            this.DataNameLabel.Text = "資料名稱：";
            // 
            // DataNameTextBox
            // 
            this.DataNameTextBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataNameTextBox.Location = new System.Drawing.Point(17, 43);
            this.DataNameTextBox.Name = "DataNameTextBox";
            this.DataNameTextBox.Size = new System.Drawing.Size(294, 43);
            this.DataNameTextBox.TabIndex = 1;
            this.DataNameTextBox.TextChanged += new System.EventHandler(this.DataNameTextBox_TextChanged);
            // 
            // DataTypeLabel
            // 
            this.DataTypeLabel.AutoSize = true;
            this.DataTypeLabel.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataTypeLabel.Location = new System.Drawing.Point(17, 92);
            this.DataTypeLabel.Name = "DataTypeLabel";
            this.DataTypeLabel.Size = new System.Drawing.Size(163, 30);
            this.DataTypeLabel.TabIndex = 0;
            this.DataTypeLabel.Text = "資料類型：";
            // 
            // DataTypeComboBox
            // 
            this.DataTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataTypeComboBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataTypeComboBox.FormattingEnabled = true;
            this.DataTypeComboBox.Items.AddRange(new object[] {
            "請選擇類型",
            "單選清單",
            "複選清單",
            "姓名",
            "數字",
            "文字",
            "地址",
            "電話",
            "郵件地址",
            "密碼",
            "日期時間",
            "日期",
            "時間"});
            this.DataTypeComboBox.Location = new System.Drawing.Point(17, 130);
            this.DataTypeComboBox.Name = "DataTypeComboBox";
            this.DataTypeComboBox.Size = new System.Drawing.Size(294, 38);
            this.DataTypeComboBox.TabIndex = 9;
            this.DataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.DataTypeComboBox_SelectedIndexChanged);
            // 
            // DataSelectItemLabel
            // 
            this.DataSelectItemLabel.AutoSize = true;
            this.DataSelectItemLabel.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataSelectItemLabel.Location = new System.Drawing.Point(17, 178);
            this.DataSelectItemLabel.Name = "DataSelectItemLabel";
            this.DataSelectItemLabel.Size = new System.Drawing.Size(163, 30);
            this.DataSelectItemLabel.TabIndex = 0;
            this.DataSelectItemLabel.Text = "資料選項：";
            // 
            // DataSelectItemListBox
            // 
            this.DataSelectItemListBox.Enabled = false;
            this.DataSelectItemListBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataSelectItemListBox.FormattingEnabled = true;
            this.DataSelectItemListBox.ItemHeight = 30;
            this.DataSelectItemListBox.Location = new System.Drawing.Point(17, 214);
            this.DataSelectItemListBox.Name = "DataSelectItemListBox";
            this.DataSelectItemListBox.Size = new System.Drawing.Size(294, 184);
            this.DataSelectItemListBox.TabIndex = 10;
            // 
            // DataSelectItemEditButton
            // 
            this.DataSelectItemEditButton.Enabled = false;
            this.DataSelectItemEditButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataSelectItemEditButton.Location = new System.Drawing.Point(186, 171);
            this.DataSelectItemEditButton.Name = "DataSelectItemEditButton";
            this.DataSelectItemEditButton.Size = new System.Drawing.Size(125, 37);
            this.DataSelectItemEditButton.TabIndex = 11;
            this.DataSelectItemEditButton.Text = "編輯選項";
            this.DataSelectItemEditButton.UseVisualStyleBackColor = true;
            this.DataSelectItemEditButton.Click += new System.EventHandler(this.DataSelectItemEditButton_Click);
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.ConfirmButton.Location = new System.Drawing.Point(221, 403);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(90, 50);
            this.ConfirmButton.TabIndex = 12;
            this.ConfirmButton.Text = "確認";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // DiscardButton
            // 
            this.DiscardButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DiscardButton.Location = new System.Drawing.Point(118, 403);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(90, 50);
            this.DiscardButton.TabIndex = 12;
            this.DiscardButton.Text = "取消";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
            // 
            // RequiredCheckBox
            // 
            this.RequiredCheckBox.Font = new System.Drawing.Font("新細明體", 14F);
            this.RequiredCheckBox.Location = new System.Drawing.Point(231, 92);
            this.RequiredCheckBox.Name = "RequiredCheckBox";
            this.RequiredCheckBox.Size = new System.Drawing.Size(80, 28);
            this.RequiredCheckBox.TabIndex = 13;
            this.RequiredCheckBox.Text = "必填";
            this.RequiredCheckBox.UseVisualStyleBackColor = true;
            this.RequiredCheckBox.CheckedChanged += new System.EventHandler(this.RequiredCheckBox_CheckedChanged);
            // 
            // DataFormatEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 462);
            this.Controls.Add(this.RequiredCheckBox);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.DataSelectItemEditButton);
            this.Controls.Add(this.DataSelectItemListBox);
            this.Controls.Add(this.DataTypeComboBox);
            this.Controls.Add(this.DataNameTextBox);
            this.Controls.Add(this.DataSelectItemLabel);
            this.Controls.Add(this.DataTypeLabel);
            this.Controls.Add(this.DataNameLabel);
            this.Font = new System.Drawing.Font("新細明體", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DataFormatEditForm";
            this.Text = "編輯資料項目";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataFormatEditForm_FormClosing);
            this.Load += new System.EventHandler(this.DataFormatEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DataNameLabel;
        private System.Windows.Forms.TextBox DataNameTextBox;
        private System.Windows.Forms.Label DataTypeLabel;
        private System.Windows.Forms.ComboBox DataTypeComboBox;
        private System.Windows.Forms.Label DataSelectItemLabel;
        private System.Windows.Forms.ListBox DataSelectItemListBox;
        private System.Windows.Forms.Button DataSelectItemEditButton;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.CheckBox RequiredCheckBox;
    }
}