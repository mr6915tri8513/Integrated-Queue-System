using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace waitlinetest_frontandqueue.Firebase
{
    public class Data
    {
        public int serviceType { get; set; }
        public Dictionary<string, string> data { get; set; }
        public string response { get; set; }
        public string message { get; set; }

    }
}
