using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using System.Text.Unicode;

namespace Json
{
    public partial class DataSetForm : Form
    {
        public DataSetForm()
        {
            InitializeComponent();
        }

        List<ServiceType> serviceTypeList = new List<ServiceType>();
        List<string> dataSelectItemList = new List<string>();
        const string addServiceTypeString = "新增...";
        //readonly string[] dataTypes = { "請選擇類型", "姓名", "數字", "文字", "已棄用", "電話", "郵件地址",
        //                                   "密碼", "日期時間", "日期", "時間"};
        const string fileName = "DataFormat.json";
        bool changed = false;

        /*const val Unknown = 0
        const val Name = 1
        const val Number = 2
        const val Text = 3
        const val Deprecated = 4
        const val Phone = 5
        const val EmailAddress = 6
        const val Password = 7
        const val DateTime = 8
        const val Date = 9
        const val Time = 10
        */

        private void SetLayout()
        {
            //DataNameTextBox.Text = DataList.Location.Y.ToString();
            DataAddButton.Location = new Point(Width - DataAddButton.Width - 25, DataAddButton.Location.Y);
            DataEditButton.Location = new Point(DataAddButton.Location.X, DataEditButton.Location.Y);
            DataRemoveButton.Location = new Point(DataAddButton.Location.X, DataRemoveButton.Location.Y);
            DataSaveButton.Location = new Point(DataAddButton.Location.X, Height - DataSaveButton.Size.Height - 50);
            DataList.Size = new Size(DataAddButton.Location.X - 28, Height - 156);
            DataList.Columns[2].Width = -2;
        }

        private void Init()
        {
            //TODO here
            if (!File.Exists(fileName))
            {
                File.CreateText(fileName).Close();
                File.WriteAllText(fileName, "[]");
            }
            string jsonString = File.ReadAllText(fileName);
            serviceTypeList = JsonSerializer.Deserialize<List<ServiceType>>(jsonString);
            /*
            serviceTypeList = JsonSerializer.Deserialize<List<ServiceType>>(jsonString, 
                new JsonSerializerOptions() { Encoder = 
                System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
                });
            */
            Debug.WriteLine(jsonString);
            if (serviceTypeList.Count != 0)
            {
                foreach (ServiceType item in serviceTypeList)
                {
                    ServiceTypeComboBox.Items.Add(item.serviceTypeName);
                }
                ServiceTypeComboBox.Items.Add(addServiceTypeString);
                ServiceTypeComboBox.SelectedIndex = 0;
                SetDataControlEnabled(true);
            }
            else
            {
                ServiceTypeComboBox.Items.Add(addServiceTypeString);
            }
        }

        private bool CheckData()
        {
            if (string.IsNullOrWhiteSpace(DataNameTextBox.Text))
            {
                return false;
            }
            switch (DataTypeComboBox.SelectedIndex)
            {
                case -1:
                case 0:
                    return false;
                case 1:
                case 2:
                    return dataSelectItemList.Count > 0;
                default:
                    return true;
            }
        }

        private void SetDataControlEnabled(bool enabled)
        {
            ServiceTypeRemoveButton.Enabled = enabled;
            DataNameTextBox.Enabled = enabled;
            DataTypeComboBox.Enabled = enabled;
            DataRequiredCheckBox.Enabled = enabled;
            DataList.Enabled = enabled;
        }

        private void SetEditRemoveEnabled(bool enabled)
        {
            //Debug.WriteLine(enabled.ToString());
            if (enabled && DataList.SelectedItems.Count != 0)
            {
                if (DataList.SelectedItems.Count > 1)
                {
                    DataEditButton.Enabled = false;
                    DataRemoveButton.Enabled = true;
                }
                else
                {
                    DataEditButton.Enabled = true;
                    DataRemoveButton.Enabled = true;
                }
            }
            else
            {
                DataEditButton.Enabled = false;
                DataRemoveButton.Enabled = false;
            }
        }

        private void AddDataFormatToListView(DataFormat dataFormat)
        {
            ListViewItem item = new ListViewItem(new string[] {
                    dataFormat.dataRequired? "是" : "否",
                    dataFormat.dataName,
                    DataTypeComboBox.Items[dataFormat.dataType].ToString(),
                    dataFormat.dataType == 1 || dataFormat.dataType == 2?
                    string.Join(", ", dataFormat.dataSelectItems) : "無"
                });
            DataList.Items.Add(item);
        }

        private void SetDataFormatInListView(DataFormat dataFormat, int index)
        {
            ListViewItem item = new ListViewItem(new string[] {
                    dataFormat.dataRequired? "是" : "否",
                    dataFormat.dataName,
                    DataTypeComboBox.Items[dataFormat.dataType].ToString(),
                    dataFormat.dataType == 1 || dataFormat.dataType == 2?
                    string.Join(", ", dataFormat.dataSelectItems) : "無"
                });
            DataList.Items[index] = item;
        }

        private void EditData(int index)
        {
            DataFormatEditForm dataFormatEditForm = new DataFormatEditForm();
            dataFormatEditForm.ShowDialog(this);
            changed = true;
            DataSaveButton.Enabled = true;
        }

        private void SaveData()
        {
            Debug.WriteLine("SaveData");
            DataSaveButton.Enabled = false;
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(serviceTypeList, options);
            if (!File.Exists(fileName))
            {
                File.CreateText(fileName).Close();
            }
            File.WriteAllText(fileName, jsonString);
            changed = false;
        }
        
        public List<string> RequestServiceTypeNameList()
        {
            return serviceTypeList.ConvertAll(new Converter<ServiceType, string> ((serviceType) => {
                return serviceType.serviceTypeName;
            }));
        }

        public void AddServiceType(string serviceType, int adoptionDataFormatIndex)
        {
            List<DataFormat> dataFormats = new List<DataFormat>();
            if (adoptionDataFormatIndex > -1)
            {
                dataFormats = serviceTypeList[adoptionDataFormatIndex].dataFormats;
            }
            serviceTypeList.Add(new ServiceType
            {
                serviceTypeName = serviceType,
                dataFormats = dataFormats
            });
            ServiceTypeComboBox.Items.Add(addServiceTypeString);
            ServiceTypeComboBox.Items[ServiceTypeComboBox.SelectedIndex] = serviceType;
            changed = true;
            DataSaveButton.Enabled = true;
        }

        public void AddServiceTypeCanceld()
        {
            if (ServiceTypeComboBox.Items.Count != 0)
            {
                ServiceTypeComboBox.SelectedIndex = 0;
            }
            else
            {
                ServiceTypeComboBox.SelectedIndex = -1;
            }
        }

        public List<string> GetDataSelectItemList()
        {
            return new List<string>(dataSelectItemList);
        }

        public void SetDataSelectItemList(List<string> dataSelectItemList)
        {
            this.dataSelectItemList = new List<string>(dataSelectItemList);
        }

        public DataFormat GetEditDataFormat()
        {
            return new DataFormat(
                serviceTypeList[ServiceTypeComboBox.SelectedIndex]
                .dataFormats[DataList.SelectedIndices[0]]);
        }

        public void SetEditDataFormat(DataFormat dataFormat)
        {
            serviceTypeList[ServiceTypeComboBox.SelectedIndex]
                .dataFormats[DataList.SelectedIndices[0]] = new DataFormat(dataFormat);
            SetDataFormatInListView(dataFormat, DataList.SelectedIndices[0]);
        }

        private void DataSetForm_Load(object sender, EventArgs e)
        {
            SetLayout();
            Init();
        }

        private void DataSetForm_ClientSizeChanged(object sender, EventArgs e)
        {
            SetLayout();
        }

        private void DataSetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                DialogResult dialogResult = MessageBox.Show("是否儲存變更？", "儲存變更", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveData();
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ServiceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(ServiceTypeComboBox.SelectedIndex.ToString());
            if (ServiceTypeComboBox.SelectedIndex == ServiceTypeComboBox.Items.Count - 1)
            {
                AddServiceTypeForm addServiceTypeForm = new AddServiceTypeForm();
                addServiceTypeForm.ShowDialog(this);
            }
            else
            {
                int index = ServiceTypeComboBox.SelectedIndex;
                DataList.Items.Clear();
                foreach (DataFormat dataFormat in serviceTypeList[index].dataFormats)
                {
                    AddDataFormatToListView(dataFormat);
                }
                SetDataControlEnabled(true);
            }
        }

        private void DataTypeRemoveButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"確定要刪除此類型\n「{ServiceTypeComboBox.SelectedItem}」？\n(此動作無法復原)", "刪除服務類型", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                serviceTypeList.RemoveAt(ServiceTypeComboBox.SelectedIndex);
                ServiceTypeComboBox.Items.RemoveAt(ServiceTypeComboBox.SelectedIndex);
                changed = true;
                DataSaveButton.Enabled = true;
                if (serviceTypeList.Count != 0)
                {
                    ServiceTypeComboBox.SelectedIndex = 0;
                }
                else
                {
                    SetDataControlEnabled(false);
                }
            }
        }

        private void DataNameTextBox_TextChanged(object sender, EventArgs e)
        {
            DataAddButton.Enabled = CheckData();
        }

        private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataAddButton.Enabled = CheckData();
            int index = DataTypeComboBox.SelectedIndex;
            DataSelectItemEditButton.Enabled = index == 1 || index == 2;
        }

        private void DataSelectItemEditButton_Click(object sender, EventArgs e)
        {
            DataSelectItemEditForm itemEditForm = new DataSelectItemEditForm();
            itemEditForm.ShowDialog(this);
            DataAddButton.Enabled = CheckData();
        }

        private void DataRequiredCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DataAddButton_Click(object sender, EventArgs e)
        {
            if (CheckData())
            {
                DataFormat dataFormat = new DataFormat
                {
                    dataName = DataNameTextBox.Text,
                    dataType = DataTypeComboBox.SelectedIndex,
                    dataSelectItems = new List<string>(dataSelectItemList),
                    dataRequired = DataRequiredCheckBox.Checked
                };
                serviceTypeList[ServiceTypeComboBox.SelectedIndex].dataFormats.Add(dataFormat);
                AddDataFormatToListView(dataFormat);
                DataNameTextBox.Text = string.Empty;
                dataSelectItemList.Clear();
                DataTypeComboBox.SelectedIndex = -1;
                changed = true;
                DataSaveButton.Enabled = true;
            }
        }

        private void DataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEditRemoveEnabled(true);
        }

        private void DataList_DoubleClick(object sender, EventArgs e)
        {
            if (DataList.SelectedIndices.Count == 1)
            {
                EditData(DataList.SelectedIndices[0]);
            }
        }

        private void DataEditButton_Click(object sender, EventArgs e)
        {
            if (DataList.SelectedIndices.Count == 1)
            {
                EditData(DataList.SelectedIndices[0]);
            }
        }

        private void DataRemoveButton_Click(object sender, EventArgs e)
        {
            if (DataList.SelectedIndices.Count > 0)
            {   
                DialogResult dialogResult = MessageBox.Show($"是否要刪除這 {DataList.SelectedIndices.Count} 個項目？", "刪除項目", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    for (int i = DataList.SelectedIndices.Count - 1; i > -1; i--)
                    {
                        serviceTypeList[ServiceTypeComboBox.SelectedIndex].dataFormats.RemoveAt(DataList.SelectedIndices[i]);
                        DataList.Items[DataList.SelectedIndices[i]].Remove();
                    }
                    changed = true;
                    DataSaveButton.Enabled = true;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    DataList.SelectedItems.Clear();
                }
            }
        }

        private void DataSaveButton_Click(object sender, EventArgs e)
        {
            SaveData();
        }

    }
}
