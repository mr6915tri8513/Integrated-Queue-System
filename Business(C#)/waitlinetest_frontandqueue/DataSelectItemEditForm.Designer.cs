
namespace waitlinetest_frontandqueue
{
    partial class DataSelectItemEditForm
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
            this.DataItemListBox = new System.Windows.Forms.ListBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.DataItemTextBox = new System.Windows.Forms.TextBox();
            this.EditButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DataItemListBox
            // 
            this.DataItemListBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataItemListBox.FormattingEnabled = true;
            this.DataItemListBox.ItemHeight = 30;
            this.DataItemListBox.Location = new System.Drawing.Point(12, 61);
            this.DataItemListBox.Name = "DataItemListBox";
            this.DataItemListBox.Size = new System.Drawing.Size(672, 364);
            this.DataItemListBox.TabIndex = 0;
            this.DataItemListBox.SelectedIndexChanged += new System.EventHandler(this.DataItemListBox_SelectedIndexChanged);
            this.DataItemListBox.DoubleClick += new System.EventHandler(this.DataItemListBox_DoubleClick);
            // 
            // AddButton
            // 
            this.AddButton.Enabled = false;
            this.AddButton.Font = new System.Drawing.Font("新細明體", 18F);
            this.AddButton.Location = new System.Drawing.Point(698, 61);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(90, 50);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "新增";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Font = new System.Drawing.Font("新細明體", 18F);
            this.RemoveButton.Location = new System.Drawing.Point(698, 173);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(90, 50);
            this.RemoveButton.TabIndex = 2;
            this.RemoveButton.Text = "刪除";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Enabled = false;
            this.ConfirmButton.Font = new System.Drawing.Font("新細明體", 18F);
            this.ConfirmButton.Location = new System.Drawing.Point(698, 388);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(90, 50);
            this.ConfirmButton.TabIndex = 2;
            this.ConfirmButton.Text = "確定";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // DataItemTextBox
            // 
            this.DataItemTextBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataItemTextBox.Location = new System.Drawing.Point(13, 13);
            this.DataItemTextBox.Name = "DataItemTextBox";
            this.DataItemTextBox.Size = new System.Drawing.Size(671, 43);
            this.DataItemTextBox.TabIndex = 3;
            this.DataItemTextBox.TextChanged += new System.EventHandler(this.DataItemTextBox_TextChanged);
            this.DataItemTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataItemTextBox_KeyDown);
            // 
            // EditButton
            // 
            this.EditButton.Enabled = false;
            this.EditButton.Font = new System.Drawing.Font("新細明體", 18F);
            this.EditButton.Location = new System.Drawing.Point(698, 117);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(90, 50);
            this.EditButton.TabIndex = 4;
            this.EditButton.Text = "編輯";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // DataSelectItemEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.DataItemTextBox);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.DataItemListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DataSelectItemEditForm";
            this.Text = "資料選項編輯器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataSetForm_FormClosing);
            this.Load += new System.EventHandler(this.ItemEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox DataItemListBox;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.TextBox DataItemTextBox;
        private System.Windows.Forms.Button EditButton;
    }
}