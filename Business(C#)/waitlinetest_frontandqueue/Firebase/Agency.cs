using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace waitlinetest_frontandqueue.Firebase
{
    public class Agency
    {
        public string address { get; set; }
        public Dictionary<string, int> datatype { get; set; }
        public string id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string name { get; set; }
        public string next_num { get; set; }
        public int pending { get; set; }
        public Dictionary<string, Data> queue { get; set; }
        public int status { get; set; }
        public int type { get; set; }
    }
}
