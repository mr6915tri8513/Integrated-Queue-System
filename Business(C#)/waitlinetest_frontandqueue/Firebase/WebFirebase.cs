using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace waitlinetest_frontandqueue.Firebase
{
    class WebFirebase
    {
        private IFirebaseClient ifclient;
        public string Agency_ID = "";
        public WebFirebase(string Agency_ID)
        {
            this.Agency_ID = Agency_ID;
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "g3Qthid6Kq9OYL2fNGE1aOv45lFJHwFON1dtExDA",
                BasePath = "https://integrated-queue-system-api-default-rtdb.asia-southeast1.firebasedatabase.app"
            };
            ifclient = new FirebaseClient(config);
        }
        public async Task set_firebase_data(string aim, string data)
        {
 
            await ifclient.SetAsync(aim, data);
        }
        public async Task set_firebase_data(string aim, int data)
        {
            await ifclient.SetAsync(aim, data);
        }
        public async Task set_data_format(List<ServiceType> serviceTypeList)
        {
            await ifclient.SetAsync(Agency_ID + "dataFormat", serviceTypeList);
        }
        public async Task<Dictionary<String, Data>> get_queue()
        {
            FirebaseResponse response = await ifclient.GetAsync(Agency_ID + "queue");//TODO/如果去掉記得
            return response.ResultAs<Dictionary<String, Data>>(); 
        }
    }
}
