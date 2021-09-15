using System.Collections.Generic;

namespace SecKill.Model
{
    public class Area
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public List<Area> Children { get; set; }
    }
}
