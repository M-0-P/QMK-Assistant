using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class KeyboardLayer : ICloneable
    {
        public KeyboardLayer()
        {
        }

        public KeyboardLayer(int i)
        {
            Priority = i;
        }

        public int Priority { get; set; }

        public string Name { get; set; }

        public string ColorHex { get; set; } = "#FF303030";

        public string Description { get; set; }

        public object Clone()
        {
            KeyboardLayer l = new KeyboardLayer(Priority);
            l.Name = Name;
            l.ColorHex = ColorHex;
            l.Description = Description;
            return l;
        }

    }
}
