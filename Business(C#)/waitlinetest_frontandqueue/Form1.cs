using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using waitlinetest_frontandqueue.Firebase;
using System.Drawing.Printing;
using System.Text.Json;

namespace waitlinetest_frontandqueue
{
    public partial class Form1 : Form
    {
        private Form form2 = new Form(); //users UI form
        private Form3 form3 = new Form3();
        private DataSetForm DataSetForm = new DataSetForm();

        readonly List<List<int>> waitlines = new List<List<int>>();
        readonly List<List<int>> canusers = new List<List<int>>();
        readonly List<int> counterServiceType = new List<int> { 0, 1 };
        private List<string> serving = new List<string>();
        private List<ServiceType> serviceTypeList = new List<ServiceType>();
        private List<int> people = new List<int>(); //waiting people
        private List<string> monitorDisplay = new List<string>();

        private delegate void bnTitleChange(int serviceType, string text);
        private string Agency_ID = "02001/";
        private string NowTime = string.Empty;
        private string text = "機構名稱 排隊服務";//"歡迎來到Integrated Queue System 模擬 測試中......";
        private WebFirebase webfirebase;
        portCommunication display = new portCommunication();
        Label Mlabel1;
        Label Mlabel2;
        Label Mlabel3;
        Label Mlabel4;
        Label Mlabel5;
        Label Mlabel6;
        private bool voiceStatus = false;
        private string voicePath = @"D:\IQS_voice\";
        private List<string> voiceQueue = new List<string>();
        private int timeCount = 0;
        public Form1()
        {
            display.port.Open();
            InitializeComponent();
            setUIform();
            form3.form1 = this;
            form3.Show();
            DataSetForm.Show();

            Mlabel1 = (Label)this.form3.Controls.Find("label1", true)[0];
            Mlabel2 = (Label)this.form3.Controls.Find("label2", true)[0];
            Mlabel3 = (Label)this.form3.Controls.Find("label3", true)[0];
            Mlabel4 = (Label)this.form3.Controls.Find("label4", true)[0];
            Mlabel5 = (Label)this.form3.Controls.Find("label5", true)[0];
            Mlabel6 = (Label)this.form3.Controls.Find("label6", true)[0];
            for(int i  = 0; i < 6; i++)
            {
                monitorDisplay.Add("");
            }
            display.port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(dataReceived);
            display.LED_display("IQS loading..,");
            webfirebase = new WebFirebase(Agency_ID);

            //TODO transfort it to function
            Debug.WriteLine(Application.StartupPath);
            string fileName = "DataFormat.json";
            if (!File.Exists(fileName)) {
                File.CreateText(fileName).Close();
                File.WriteAllText(fileName, "[]");
                //這個炸了就沒了
            }
            string jsonString = File.ReadAllText(fileName);
            serviceTypeList = JsonSerializer.Deserialize<List<ServiceType>>(jsonString);
            webfirebase.set_data_format(serviceTypeList);
            webfirebase.set_firebase_data(Agency_ID + "status", 5);
            for (int i = 0; i < serviceTypeList.Count; i++)
            {
                webfirebase.set_firebase_data(Agency_ID + $"next_num/{i}", GetNum(i, 0));
                waitlines.Add(new List<int>());
                canusers.Add(new List<int>());
                serving.Add(null);
                people.Add(0);
            }
            timer2.Enabled = true;
            timer2.Interval = 5000;
            timer2.Start();
            check();

            voiceQueue.Add("0");
            voiceStatus = true;
        }
        private void dataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)//TODO static
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            switch (sp.ReadExisting().ToString())
            {
                case "s01":
                    serverBN(1);
                    break;
                case "w01":
                    stopBN(1);
                    break;
                case "s02":
                    serverBN(2);
                    break;
                case "w02":
                    stopBN(2);
                    break;
            };
            Thread.Sleep(20);
        }
        private void print_ticket(string num)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = "TX 80 Thermal";
            pd.PrintPage += (sender, args) => pd_PrintPage(num, args);
            pd.Print();
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            string num = sender as string;
            string print_str = "IQS 排隊服務系統";
            Font printFont = new Font("Arial", 20);
            ev.Graphics.DrawString(print_str,printFont, Brushes.Black, 0f, 0f, new StringFormat());
            if (num != null)
            {
                print_str = num;
            }
            else
            {
                print_str = "A000";
            }
            printFont = new Font("Arial", 28);
            ev.Graphics.DrawString(print_str, printFont, Brushes.Black, 80f, 40f, new StringFormat());
            printFont = new Font("Arial", 16);
            ev.Graphics.DrawString(NowTime, printFont, Brushes.Black, 0f, 90f, new StringFormat());
            ev.Graphics.DrawString(".", printFont, Brushes.Black, 0f, 150f, new StringFormat());
            ev.HasMorePages = false;
        }
        private string GetNum(int serviceType, int people)
        {
            return Convert.ToChar(serviceType + 65) + string.Format("{0:000}", people);
        }
        private void check()//我覺得這個邏輯要改一下
        {
            for (int i = 0; i < serviceTypeList.Count; i++)
            {
                if (canusers[i].Count != 0 && waitlines[i].Count != 0) //if the system have people waiting and staffs waitng together ,give people to staff  
                {
                    int wait = waitlines[i][0];
                    int user = canusers[i][0] - 1;
                    serving[user] = GetNum(i, wait); 
                    display.LED_display(string.Format("Please No. {0:000} to No. {1:000} counter", wait, canusers[i][0]));
                    webfirebase.set_firebase_data(Agency_ID + $"next_num/{i}", serving[user]);
                    monitorDisplay.Add(string.Format("請{0}{1:000}到{2:000}號櫃台", Convert.ToChar(i + 65), waitlines[i][0], canusers[i][0]));
                    voiceQueue.AddRange(new List<string> { "請", i == 1 ? "B" : "A", Convert.ToString(wait / 100), Convert.ToString((wait / 10) % 10), Convert.ToString(wait % 10),
                        "號到", Convert.ToString(canusers[i][0] / 100), Convert.ToString((canusers[i][0] / 10) % 10), Convert.ToString(canusers[i][0] % 10), "號","櫃台" });
                    voiceStatus = true;
                    //Thread sound = new Thread(new ParameterizedThreadStart(voice));
                   // sound.Start(new List<string> { "請",i == 1?"B":"A", Convert.ToString(wait/100), Convert.ToString((wait / 10)%10), Convert.ToString(wait % 10),"號到", Convert.ToString(canusers[i][0] / 100), Convert.ToString((canusers[i][0] / 10) % 10), Convert.ToString(canusers[i][0] % 10), "號櫃台" });

                    waitlines[i].RemoveAt(0);
                    canusers[i].RemoveAt(0);
                    
                }
                string msga = serviceTypeList[i].serviceTypeName + '\n' + "目前等待人數：" + waitlines[i].Count.ToString() + "位 ";
                if (this.InvokeRequired)
                {
                    bnTitleChange bn = new bnTitleChange(changeText);
                    this.Invoke(bn, i, msga);
                }
                else
                {
                    if (i == 0)
                    {
                        this.GetTicketButton1.Text = msga;
                    }
                    else if (i == 1)
                    {
                        this.GetTicketButton2.Text = msga;
                    }
                    while (monitorDisplay.Count > 6)
                    {
                        monitorDisplay.RemoveAt(0);
                    }
                    Mlabel1.Text = monitorDisplay[5];
                    Mlabel2.Text = monitorDisplay[4];
                    Mlabel3.Text = monitorDisplay[3];
                    Mlabel4.Text = monitorDisplay[2];
                    Mlabel5.Text = monitorDisplay[1];
                    Mlabel6.Text = monitorDisplay[0];
                }
                
                //TODO format changed
                /*
                using (StreamReader sr = File.OpenText(@"..\..\Queue_Data\Data.txt"))
                {
                    string str = sr.ReadToEnd();
                }
                using (StreamWriter sw = new StreamWriter(@"..\..\Queue_Data\Data.txt"))
                {
                    sw.WriteLine(msga);
                    sw.WriteLine("目前叫號號碼 ： " + waitl);
                }*/
                //TODO
                /*
                string path = @"..\..\Queue_Data\Data.json";
                if (!File.Exists(path))
                {
                    File.CreateText(path);
                }*/
                webfirebase.set_firebase_data(Agency_ID + $"pending/{i}", waitlines[i].Count);
            }
            for (int i = 0; i < counterServiceType.Count; i++)
            {
                display.arduino_refresh(i + 1, (serving[i] != null? serving[i] : "A000") + 
                    string.Format("{0:000}", waitlines[counterServiceType[i]].Count));
            }
            GC.Collect();
        }
        private void voice(object obj)
        {
            List<string> voiceList = (List<string>)obj;
            string path = @"D:\IQS_voice\";
            player.settings.autoStart = true;
            /*foreach (string word in voiceList)
            {
                player.URL = path + word + ".mp3";
                
                //while (player.playState != WMPLib.WMPPlayState.wmppsStopped) { }
                    //Application.DoEvents();
            }*/
        }
        private void changeText(int serviceType, string text)
        {
            if (serviceType == 0)
            {
                GetTicketButton1.Text = text;
            }
            else if (serviceType == 1)
            {
                GetTicketButton2.Text = text;
            }
            while (monitorDisplay.Count > 6)
            {
                monitorDisplay.RemoveAt(0);
            }
            Mlabel1.Text = monitorDisplay[5];
            Mlabel2.Text = monitorDisplay[4];
            Mlabel3.Text = monitorDisplay[3];
            Mlabel4.Text = monitorDisplay[2];
            Mlabel5.Text = monitorDisplay[1];
            Mlabel6.Text = monitorDisplay[0];
        }
        private void AddWaiting(int serviceType)
        {
            people[serviceType]++;
            waitlines[serviceType].Add(people[serviceType]);
            print_ticket(GetNum(serviceType, people[serviceType]));
            check();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            NowTime = DateTime.Now.ToString().Replace(" ", "");
            ShowUItime.Text = NowTime;
            ShowUImsg.Left = ShowUImsg.Left < (-text.Length * 32) ? form2.Width : ShowUImsg.Left - 1; //moving label show the Business title
            if (voiceQueue.Count != 0)
            {
                if (timeCount == 0)
                {
                    player.URL = voicePath + voiceQueue[0] + ".mp3";
                    player.settings.autoStart = true;
                }
                timeCount++;
                if (timeCount > 50)
                {
                    timeCount = 0;
                    voiceQueue.RemoveAt(0);
                }
                
            }
            else
            {
                //player.settings.autoStart = false;
            }
        
        }
        private bool checkData(Data data)
        {
            return true;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            var task = Task.Run<Dictionary<String, Data>>(webfirebase.get_queue);
            //TODO no internet
            if (task.Result != null)
            {
                foreach (var reserve in task.Result)
                {
                    if (reserve.Value.response == "0") //C端
                    {
                        if (checkData(reserve.Value))
                        {
                            int serviceType = reserve.Value.serviceType;
                            people[serviceType]++;
                            waitlines[serviceType].Add(people[serviceType]);
                            webfirebase.set_firebase_data($"{Agency_ID}queue/{reserve.Key}/response", GetNum(serviceType, people[serviceType]));
                            check();
                        }
                        else
                        {
                            webfirebase.set_firebase_data($"{Agency_ID}queue/{reserve.Key}/message", "Reservation not Accept");
                            webfirebase.set_firebase_data($"{Agency_ID}queue/{reserve.Key}/response", "No");
                        }
                    }
                }
            }
        }

        public void serverBN(int code)
        {
            int serviceType = counterServiceType[code - 1];
            if (!canusers[serviceType].Contains(code))
            {
                canusers[serviceType].Add(code);
                check();
            }
        }
        public void stopBN(int code)
        {
            int serviceType = counterServiceType[code - 1];
            if (canusers[serviceType].Contains(code))
            {
                while (canusers[serviceType].Contains(code))
                {
                    canusers[serviceType].Remove(code);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            serverBN(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            serverBN(2);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (button1.Enabled == false)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
            stopBN(1);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (button2.Enabled == false)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
            stopBN(2);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            webfirebase.set_firebase_data(Agency_ID + "status", 2);
        }
    }
}
