
namespace waitlinetest_frontandqueue
{
    partial class DataSetForm
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.DataList = new System.Windows.Forms.ListView();
            this.DataName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataSelectItems = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataAddButton = new System.Windows.Forms.Button();
            this.DataNameTextBox = new System.Windows.Forms.TextBox();
            this.DataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.DataSaveButton = new System.Windows.Forms.Button();
            this.ServiceTypeRemoveButton = new System.Windows.Forms.Button();
            this.DataEditButton = new System.Windows.Forms.Button();
            this.DataRemoveButton = new System.Windows.Forms.Button();
            this.DataSelectItemEditButton = new System.Windows.Forms.Button();
            this.DataRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.DataRequired = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ServiceTypeComboBox
            // 
            this.ServiceTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServiceTypeComboBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.ServiceTypeComboBox.FormattingEnabled = true;
            this.ServiceTypeComboBox.Location = new System.Drawing.Point(13, 13);
            this.ServiceTypeComboBox.Name = "ServiceTypeComboBox";
            this.ServiceTypeComboBox.Size = new System.Drawing.Size(264, 38);
            this.ServiceTypeComboBox.TabIndex = 3;
            this.ServiceTypeComboBox.Tag = "";
            this.ServiceTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ServiceTypeComboBox_SelectedIndexChanged);
            // 
            // DataList
            // 
            this.DataList.AllowColumnReorder = true;
            this.DataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DataRequired,
            this.DataName,
            this.DataType,
            this.DataSelectItems});
            this.DataList.Enabled = false;
            this.DataList.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataList.FullRowSelect = true;
            this.DataList.GridLines = true;
            this.DataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.DataList.HideSelection = false;
            this.DataList.Location = new System.Drawing.Point(13, 106);
            this.DataList.Name = "DataList";
            this.DataList.Size = new System.Drawing.Size(669, 332);
            this.DataList.TabIndex = 5;
            this.DataList.UseCompatibleStateImageBehavior = false;
            this.DataList.View = System.Windows.Forms.View.Details;
            this.DataList.SelectedIndexChanged += new System.EventHandler(this.DataList_SelectedIndexChanged);
            this.DataList.DoubleClick += new System.EventHandler(this.DataList_DoubleClick);
            // 
            // DataName
            // 
            this.DataName.Text = "資料名稱";
            this.DataName.Width = 200;
            // 
            // DataType
            // 
            this.DataType.Text = "資料類型";
            this.DataType.Width = 150;
            // 
            // DataSelectItems
            // 
            this.DataSelectItems.Text = "資料選項";
            this.DataSelectItems.Width = 250;
            // 
            // DataAddButton
            // 
            this.DataAddButton.Enabled = false;
            this.DataAddButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataAddButton.Location = new System.Drawing.Point(688, 40);
            this.DataAddButton.Name = "DataAddButton";
            this.DataAddButton.Size = new System.Drawing.Size(100, 60);
            this.DataAddButton.TabIndex = 6;
            this.DataAddButton.Text = "新增\n項目";
            this.DataAddButton.UseVisualStyleBackColor = true;
            this.DataAddButton.Click += new System.EventHandler(this.DataAddButton_Click);
            // 
            // DataNameTextBox
            // 
            this.DataNameTextBox.Enabled = false;
            this.DataNameTextBox.Font = new System.Drawing.Font("新細明體", 18F);
            this.DataNameTextBox.Location = new System.Drawing.Point(13, 57);
            this.DataNameTextBox.Name = "DataNameTextBox";
            this.DataNameTextBox.Size = new System.Drawing.Size(264, 43);
            this.DataNameTextBox.TabIndex = 7;
            this.DataNameTextBox.TextChanged += new System.EventHandler(this.DataNameTextBox_TextChanged);
            // 
            // DataTypeComboBox
            // 
            this.DataTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataTypeComboBox.Enabled = false;
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
            this.DataTypeComboBox.Location = new System.Drawing.Point(283, 62);
            this.DataTypeComboBox.Name = "DataTypeComboBox";
            this.DataTypeComboBox.Size = new System.Drawing.Size(167, 38);
            this.DataTypeComboBox.TabIndex = 8;
            this.DataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.DataTypeComboBox_SelectedIndexChanged);
            // 
            // DataSaveButton
            // 
            this.DataSaveButton.Enabled = false;
            this.DataSaveButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataSaveButton.Location = new System.Drawing.Point(688, 378);
            this.DataSaveButton.Name = "DataSaveButton";
            this.DataSaveButton.Size = new System.Drawing.Size(100, 60);
            this.DataSaveButton.TabIndex = 9;
            this.DataSaveButton.Text = "儲存\n變更";
            this.DataSaveButton.UseVisualStyleBackColor = true;
            this.DataSaveButton.Click += new System.EventHandler(this.DataSaveButton_Click);
            // 
            // ServiceTypeRemoveButton
            // 
            this.ServiceTypeRemoveButton.Enabled = false;
            this.ServiceTypeRemoveButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.ServiceTypeRemoveButton.Location = new System.Drawing.Point(283, 13);
            this.ServiceTypeRemoveButton.Name = "ServiceTypeRemoveButton";
            this.ServiceTypeRemoveButton.Size = new System.Drawing.Size(188, 38);
            this.ServiceTypeRemoveButton.TabIndex = 10;
            this.ServiceTypeRemoveButton.Text = "刪除此服務類型";
            this.ServiceTypeRemoveButton.UseVisualStyleBackColor = true;
            this.ServiceTypeRemoveButton.Click += new System.EventHandler(this.DataTypeRemoveButton_Click);
            // 
            // DataEditButton
            // 
            this.DataEditButton.Enabled = false;
            this.DataEditButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataEditButton.Location = new System.Drawing.Point(688, 106);
            this.DataEditButton.Name = "DataEditButton";
            this.DataEditButton.Size = new System.Drawing.Size(100, 60);
            this.DataEditButton.TabIndex = 6;
            this.DataEditButton.Text = "編輯\n項目";
            this.DataEditButton.UseVisualStyleBackColor = true;
            this.DataEditButton.Click += new System.EventHandler(this.DataEditButton_Click);
            // 
            // DataRemoveButton
            // 
            this.DataRemoveButton.Enabled = false;
            this.DataRemoveButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataRemoveButton.Location = new System.Drawing.Point(688, 172);
            this.DataRemoveButton.Name = "DataRemoveButton";
            this.DataRemoveButton.Size = new System.Drawing.Size(100, 60);
            this.DataRemoveButton.TabIndex = 6;
            this.DataRemoveButton.Text = "刪除\n項目";
            this.DataRemoveButton.UseVisualStyleBackColor = true;
            this.DataRemoveButton.Click += new System.EventHandler(this.DataRemoveButton_Click);
            // 
            // DataSelectItemEditButton
            // 
            this.DataSelectItemEditButton.Enabled = false;
            this.DataSelectItemEditButton.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataSelectItemEditButton.Location = new System.Drawing.Point(456, 57);
            this.DataSelectItemEditButton.Name = "DataSelectItemEditButton";
            this.DataSelectItemEditButton.Size = new System.Drawing.Size(115, 38);
            this.DataSelectItemEditButton.TabIndex = 11;
            this.DataSelectItemEditButton.Text = "編輯選項";
            this.DataSelectItemEditButton.UseVisualStyleBackColor = true;
            this.DataSelectItemEditButton.Click += new System.EventHandler(this.DataSelectItemEditButton_Click);
            // 
            // DataRequiredCheckBox
            // 
            this.DataRequiredCheckBox.AutoSize = true;
            this.DataRequiredCheckBox.Enabled = false;
            this.DataRequiredCheckBox.Font = new System.Drawing.Font("新細明體", 14F);
            this.DataRequiredCheckBox.Location = new System.Drawing.Point(577, 62);
            this.DataRequiredCheckBox.Name = "DataRequiredCheckBox";
            this.DataRequiredCheckBox.Size = new System.Drawing.Size(80, 28);
            this.DataRequiredCheckBox.TabIndex = 12;
            this.DataRequiredCheckBox.Text = "必填";
            this.DataRequiredCheckBox.UseVisualStyleBackColor = true;
            this.DataRequiredCheckBox.CheckedChanged += new System.EventHandler(this.DataRequiredCheckBox_CheckedChanged);
            // 
            // DataRequired
            // 
            this.DataRequired.Text = "必填";
            this.DataRequired.Width = 75;
            // 
            // DataSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DataRequiredCheckBox);
            this.Controls.Add(this.DataSelectItemEditButton);
            this.Controls.Add(this.ServiceTypeRemoveButton);
            this.Controls.Add(this.DataSaveButton);
            this.Controls.Add(this.DataTypeComboBox);
            this.Controls.Add(this.DataNameTextBox);
            this.Controls.Add(this.DataRemoveButton);
            this.Controls.Add(this.DataEditButton);
            this.Controls.Add(this.DataAddButton);
            this.Controls.Add(this.DataList);
            this.Controls.Add(this.ServiceTypeComboBox);
            this.Font = new System.Drawing.Font("新細明體", 9F);
            this.MinimumSize = new System.Drawing.Size(818, 497);
            this.Name = "DataSetForm";
            this.Text = "資料格式設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataSetForm_FormClosing);
            this.Load += new System.EventHandler(this.DataSetForm_Load);
            this.ClientSizeChanged += new System.EventHandler(this.DataSetForm_ClientSizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox ServiceTypeComboBox;
        private System.Windows.Forms.ListView DataList;
        private System.Windows.Forms.Button DataAddButton;
        private System.Windows.Forms.ColumnHeader DataName;
        private System.Windows.Forms.ColumnHeader DataType;
        private System.Windows.Forms.ColumnHeader DataSelectItems;
        private System.Windows.Forms.TextBox DataNameTextBox;
        private System.Windows.Forms.ComboBox DataTypeComboBox;
        private System.Windows.Forms.Button DataSaveButton;
        private System.Windows.Forms.Button ServiceTypeRemoveButton;
        private System.Windows.Forms.Button DataEditButton;
        private System.Windows.Forms.Button DataRemoveButton;
        private System.Windows.Forms.Button DataSelectItemEditButton;
        private System.Windows.Forms.CheckBox DataRequiredCheckBox;
        private System.Windows.Forms.ColumnHeader DataRequired;
    }
}