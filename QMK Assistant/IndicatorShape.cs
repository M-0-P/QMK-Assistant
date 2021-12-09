using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class IndicatorShape
    {
        public IndicatorShape()
        {

        }

        public IndicatorShape(string name, string shapepath)
        {
            Name = name;
            ShapePath = shapepath;
        }


        public string Name { get; set; }

        public string ShapePath { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
