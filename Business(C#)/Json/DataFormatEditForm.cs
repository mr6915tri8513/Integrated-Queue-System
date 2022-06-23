using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Json
{
    public partial class DataFormatEditForm : Form
    {
        public DataFormatEditForm()
        {
            InitializeComponent();
        }

        DataFormat dataFormat = new DataFormat();
        bool changed = false;
        bool save = false;

        private bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(dataFormat.dataName))
            {
                return false;
            }
            switch (dataFormat.dataType)
            {
                case -1:
                case 0:
                    return false;
                case 1:
                case 2:
                    return dataFormat.dataSelectItems.Count > 0;
                default:
                    return true;
            }
        }

        public List<string> GetDataSelectItemList()
        {
            return new List<string>(dataFormat.dataSelectItems);
        }

        public void SetDataSelectItemList(List<string> dataSelectItemList)
        {
            dataFormat.dataSelectItems = new List<string>(dataSelectItemList);
            CheckData();
            DataSelectItemListBox.Items.Clear();
            DataSelectItemListBox.Items.AddRange(dataSelectItemList.ToArray());
        }

        private void DataFormatEditForm_Load(object sender, EventArgs e)
        {
            DataSetForm parent = (DataSetForm)Owner;
            dataFormat = parent.GetEditDataFormat();
            DataNameTextBox.Text = dataFormat.dataName;
            DataTypeComboBox.SelectedIndex = dataFormat.dataType;
            RequiredCheckBox.Checked = dataFormat.dataRequired;
            if (dataFormat.dataType == 1 || dataFormat.dataType == 2)
            {
                DataSelectItemListBox.Items.AddRange(dataFormat.dataSelectItems.ToArray());
                DataSelectItemEditButton.Enabled = true;
                DataSelectItemListBox.Enabled = true;
            }
            else
            {
                DataSelectItemListBox.Items.Add("無");
            }
        }
        private void DataFormatEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save && changed)
            {
                DialogResult dialogResult = MessageBox.Show("是否儲存變更？", "儲存變更", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    save = true;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            if (save)
            {
                DataSetForm parent = (DataSetForm)Owner;
                parent.SetEditDataFormat(dataFormat);
            }
        }

        private void DataNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!changed && dataFormat.dataName != DataNameTextBox.Text)
            {
                changed = true;
            }
            dataFormat.dataName = DataNameTextBox.Text;
            ConfirmButton.Enabled = CheckData();
        }

        private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = DataTypeComboBox.SelectedIndex;
            if (!changed && dataFormat.dataType != index)
            {
                changed = true;
            }
            dataFormat.dataType = index;
            ConfirmButton.Enabled = CheckData();
            if (index == 1 || index == 2)
            {
                DataSelectItemEditButton.Enabled = true;
                DataSelectItemListBox.Enabled = true;
            }
            else
            {
                DataSelectItemEditButton.Enabled = false;
                DataSelectItemListBox.Enabled = false;
                DataSelectItemListBox.ClearSelected();
            }
        }

        private void RequiredCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!changed)
            {
                changed = true;
            }
            dataFormat.dataRequired = RequiredCheckBox.Checked;
        }

        private void DataSelectItemEditButton_Click(object sender, EventArgs e)
        {
            DataSelectItemEditForm itemEditForm = new DataSelectItemEditForm();
            itemEditForm.ShowDialog(this);
            ConfirmButton.Enabled = CheckData();
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            save = true;
            Close();
        }
    }
}
