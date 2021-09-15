using System.Collections.Generic;

namespace SecKill.Model
{
    public class VaccineDetail
    {
        public List<Date> MyProperty { get; set; }
        public long Time { get; set; }
        public long StartMilliscond { get; set; }
        public string HospitalName { get; set; }
    }

    public class Date
    {
        public string Day { get; set; }
        public int Total { get; set; }
    }
}
