
namespace waitlinetest_frontandqueue
{
    partial class AddServiceTypeForm
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
            this.ServiceTypeNameTextBox = new System.Windows.Forms.TextBox();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.NegativeButton = new System.Windows.Forms.Button();
            this.ServiceNameHintLabel = new System.Windows.Forms.Label();
            this.AdoptionDataFormatComboBox = new System.Windows.Forms.ComboBox();
            this.AdoptionDataFormatHintLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ServiceTypeNameTextBox
            // 
            this.ServiceTypeNameTextBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.ServiceTypeNameTextBox.Location = new System.Drawing.Point(40, 80);
            this.ServiceTypeNameTextBox.Name = "ServiceTypeNameTextBox";
            this.ServiceTypeNameTextBox.Size = new System.Drawing.Size(308, 43);
            this.ServiceTypeNameTextBox.TabIndex = 0;
            this.ServiceTypeNameTextBox.TextChanged += new System.EventHandler(this.ServiceTypeNameTextBox_TextChanged);
            this.ServiceTypeNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ServiceTypeNameTextBox_KeyDown);
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Enabled = false;
            this.ConfirmButton.Location = new System.Drawing.Point(263, 247);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(85, 48);
            this.ConfirmButton.TabIndex = 1;
            this.ConfirmButton.Text = "確認";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // NegativeButton
            // 
            this.NegativeButton.Location = new System.Drawing.Point(155, 247);
            this.NegativeButton.Name = "NegativeButton";
            this.NegativeButton.Size = new System.Drawing.Size(85, 48);
            this.NegativeButton.TabIndex = 1;
            this.NegativeButton.Text = "取消";
            this.NegativeButton.UseVisualStyleBackColor = true;
            this.NegativeButton.Click += new System.EventHandler(this.NegativeButton_Click);
            // 
            // ServiceNameHintLabel
            // 
            this.ServiceNameHintLabel.AutoSize = true;
            this.ServiceNameHintLabel.Font = new System.Drawing.Font("新細明體", 18F);
            this.ServiceNameHintLabel.Location = new System.Drawing.Point(35, 35);
            this.ServiceNameHintLabel.Name = "ServiceNameHintLabel";
            this.ServiceNameHintLabel.Size = new System.Drawing.Size(313, 30);
            this.ServiceNameHintLabel.TabIndex = 2;
            this.ServiceNameHintLabel.Text = "請輸入服務類型名稱：";
            // 
            // AdoptionDataFormatComboBox
            // 
            this.AdoptionDataFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AdoptionDataFormatComboBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.AdoptionDataFormatComboBox.FormattingEnabled = true;
            this.AdoptionDataFormatComboBox.Location = new System.Drawing.Point(40, 193);
            this.AdoptionDataFormatComboBox.Name = "AdoptionDataFormatComboBox";
            this.AdoptionDataFormatComboBox.Size = new System.Drawing.Size(308, 38);
            this.AdoptionDataFormatComboBox.TabIndex = 3;
            this.AdoptionDataFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.AdoptionDataFormatComboBox_SelectedIndexChanged);
            // 
            // AdoptionDataFormatHintLabel
            // 
            this.AdoptionDataFormatHintLabel.AutoSize = true;
            this.AdoptionDataFormatHintLabel.Font = new System.Drawing.Font("新細明體", 18F);
            this.AdoptionDataFormatHintLabel.Location = new System.Drawing.Point(35, 142);
            this.AdoptionDataFormatHintLabel.Name = "AdoptionDataFormatHintLabel";
            this.AdoptionDataFormatHintLabel.Size = new System.Drawing.Size(301, 30);
            this.AdoptionDataFormatHintLabel.TabIndex = 2;
            this.AdoptionDataFormatHintLabel.Text = "沿用資料格式(可選)：";
            // 
            // AddServiceTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 307);
            this.Controls.Add(this.AdoptionDataFormatComboBox);
            this.Controls.Add(this.AdoptionDataFormatHintLabel);
            this.Controls.Add(this.ServiceNameHintLabel);
            this.Controls.Add(this.NegativeButton);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.ServiceTypeNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AddServiceTypeForm";
            this.Text = "新增服務項目";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddServiceTypeForm_FormClosing);
            this.Load += new System.EventHandler(this.AddServiceTypeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServiceTypeNameTextBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button NegativeButton;
        private System.Windows.Forms.Label ServiceNameHintLabel;
        private System.Windows.Forms.ComboBox AdoptionDataFormatComboBox;
        private System.Windows.Forms.Label AdoptionDataFormatHintLabel;
    }
}