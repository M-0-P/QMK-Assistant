using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class LayerLegend : ICloneable
    {

        public LayerLegend()
        {

        }

        public LayerLegend(int priority, string name)
        {
            Priority = priority;
            Name = name;
        }


        public LayerLegend(int priority, string name, string group)
        {
            Priority = priority;
            Name = name;
            Group = group;           
        }


        public int Priority { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        public object Clone()
        {
            return new LayerLegend(Priority, Name, Group);
        }
    }
}
