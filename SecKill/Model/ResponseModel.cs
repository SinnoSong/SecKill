using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecKill.Model
{
    public class ResponseModel
    {
        public string code { get; set; }
        public Datum[] data { get; set; }
        public bool ok { get; set; }
        public bool notOk { get; set; }

        public class Datum
        {
            public int id { get; set; }
            public int userId { get; set; }
            public string name { get; set; }
            public string idCardNo { get; set; }
            public string birthday { get; set; }
            public int sex { get; set; }
            public string regionCode { get; set; }
            public string address { get; set; }
            public int isDefault { get; set; }
            public int relationType { get; set; }
            public string createTime { get; set; }
            public string modifyTime { get; set; }
            public int yn { get; set; }
            public int idCardType { get; set; }
        }
    }
}
