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
    public partial class DataSelectItemEditForm : Form
    {
        public DataSelectItemEditForm()
        {
            InitializeComponent();
        }

        List<string> dataSelectItemList = new List<string>();
        int modifyIndex = -1;
        bool changed = false;

        private void AddDataItem()
        {
            string dataItem = DataItemTextBox.Text;
            DataItemTextBox.Text = string.Empty;
            dataSelectItemList.Add(dataItem);
            DataItemListBox.Items.Add(dataItem);
            ConfirmButton.Enabled = true;
            changed = true;
        }

        private void EditItem(int index)
        {
            modifyIndex = index;
            AddButton.Text = "修改";
            EditButton.Text = "取消";
            DataItemListBox.Enabled = false;
            RemoveButton.Enabled = false;
            DataItemTextBox.Text = DataItemListBox.SelectedItem.ToString();
            DataItemListBox.SelectedItem = "(修改中)";
        }

        private void CancelEdit()
        {
            DataItemListBox.SelectedItem = dataSelectItemList[modifyIndex];
            modifyIndex = -1;
            DataItemListBox.ClearSelected();
            AddButton.Text = "新增";
            EditButton.Text = "編輯";
            DataItemListBox.Enabled = true;
            DataItemTextBox.Text = string.Empty;
        }

        private void CommitEdit()
        {
            dataSelectItemList[modifyIndex] = DataItemTextBox.Text;
            DataItemListBox.SelectedItem = DataItemTextBox.Text;
            modifyIndex = -1;
            DataItemListBox.ClearSelected();
            AddButton.Text = "新增";
            EditButton.Text = "編輯";
            DataItemListBox.Enabled = true;
            DataItemTextBox.Text = string.Empty;
            changed = true;
        }

        private void SaveDataItems()
        {
            try
            {
                DataSetForm parent = (DataSetForm)Owner;
                parent.SetDataSelectItemList(dataSelectItemList);
            }
            catch (InvalidCastException exception)
            {
                DataFormatEditForm parent = (DataFormatEditForm)Owner;
                parent.SetDataSelectItemList(dataSelectItemList);
            }
        }

        private void ItemEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                DataSetForm parent = (DataSetForm)Owner;
                dataSelectItemList = parent.GetDataSelectItemList();
                DataItemListBox.Items.AddRange(dataSelectItemList.ToArray());
                ConfirmButton.Enabled = DataItemListBox.Items.Count > 0;
            }
            catch(InvalidCastException exception)
            {
                DataFormatEditForm parent = (DataFormatEditForm)Owner;
                dataSelectItemList = parent.GetDataSelectItemList();
                DataItemListBox.Items.AddRange(dataSelectItemList.ToArray());
                ConfirmButton.Enabled = DataItemListBox.Items.Count > 0;
            }
        }
        private void DataSetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                DialogResult dialogResult = MessageBox.Show("是否儲存變更？", "儲存變更", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveDataItems();
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DataItemTextBox_TextChanged(object sender, EventArgs e)
        {
            AddButton.Enabled = !string.IsNullOrWhiteSpace(DataItemTextBox.Text);
        }

        private void DataItemTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(DataItemTextBox.Text))
                {
                    if (modifyIndex != -1)
                    {
                        CommitEdit();
                    }
                    else
                    {
                        AddDataItem();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (modifyIndex != -1)
                {
                    CancelEdit();
                }
                else
                {
                    DataItemTextBox.Text = string.Empty;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (modifyIndex != -1)
            {
                CommitEdit();
            }
            else
            {
                AddDataItem();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (DataItemListBox.SelectedIndex != -1)
            {
                if (modifyIndex != -1)
                {
                    CancelEdit();
                }
                else
                {
                    EditItem(DataItemListBox.SelectedIndex);
                }
            }
        }

        private void DataItemListBox_DoubleClick(object sender, EventArgs e)
        {
            if (DataItemListBox.SelectedIndex != -1)
            {
                EditItem(DataItemListBox.SelectedIndex);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (DataItemListBox.SelectedIndex != -1)
            {
                dataSelectItemList.RemoveAt(DataItemListBox.SelectedIndex);
                DataItemListBox.Items.RemoveAt(DataItemListBox.SelectedIndex);
                changed = true;
            }
        }

        private void DataItemListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DataItemListBox.SelectedIndex != -1)
            {
                EditButton.Enabled = true;
                RemoveButton.Enabled = true;
            }
            else
            {
                EditButton.Enabled = false;
                RemoveButton.Enabled = false;
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            SaveDataItems();
            changed = false;
            Close();
        }
    }
}
