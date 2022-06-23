using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace waitlinetest_frontandqueue
{
    public class DataFormat
    {
        public string dataName { get; set; }
        public int dataType { get; set; }
        public List<string> dataSelectItems { get; set; }
        public bool dataRequired { get; set; }

        public DataFormat()
        {

        }

        public DataFormat(DataFormat dataFormat)
        {
            dataName = dataFormat.dataName;
            dataType = dataFormat.dataType;
            dataSelectItems = (dataType == 1 || dataType == 2) && dataFormat.dataSelectItems != null?
                new List<string>(dataFormat.dataSelectItems) : new List<string>();
            dataRequired = dataFormat.dataRequired;
        }
    }
    public class ServiceType
    {
        public string serviceTypeName { get; set; }
        public List<DataFormat> dataFormats { get; set; }
    }
}
