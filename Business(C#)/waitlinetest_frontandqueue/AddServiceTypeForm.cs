using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace waitlinetest_frontandqueue
{
    public partial class AddServiceTypeForm : Form
    {
        public AddServiceTypeForm()
        {
            InitializeComponent();
        }


        List<string> serviceTypeNameList = new List<string>();
        bool comfirm = false;

        private void AddServiceTypeForm_Load(object sender, EventArgs e)
        {
            DataSetForm parent = (DataSetForm)Owner;
            serviceTypeNameList = parent.RequestServiceTypeNameList();
            AdoptionDataFormatComboBox.Items.Add("不沿用");
            AdoptionDataFormatComboBox.Items.AddRange(serviceTypeNameList.ToArray());
        }

        private void AddServiceTypeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!comfirm)
            {
                DataSetForm parent = (DataSetForm)Owner;
                parent.AddServiceTypeCanceld();
            }
        }

        private void ServiceTypeNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ConfirmButton.Enabled = !string.IsNullOrWhiteSpace(ServiceTypeNameTextBox.Text);
        }

        private void AdoptionDataFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Confirm();
        }

        private void NegativeButton_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void ServiceTypeNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Confirm();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Cancel();
            }
        }

        private void Confirm()
        {
            if (!string.IsNullOrWhiteSpace(ServiceTypeNameTextBox.Text))
            {
                DataSetForm parent = (DataSetForm)Owner;
                parent.AddServiceType(ServiceTypeNameTextBox.Text, AdoptionDataFormatComboBox.SelectedIndex - 1);
                comfirm = true;
            }
            Close();
        }

        private void Cancel()
        {
            Close();
        }
    }
}
