using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class KeyboardIndicator : ICloneable
    {

        public KeyboardIndicator()
        {
            IndicatorShape = "Square";
        }

        public KeyboardIndicator(int code, IndicatorType itype)
        {
            Code = code;
            IndicatorShape = "Square";
            Type = itype;
        }

        public int Code { get; set; }

        public string Name { get; set; }

        public string ActiveColorHex { get; set; } = "#FF000000";

        public string IndicatorShape { get; set; }

        public IndicatorType Type { get; set; }

        public List<IndicatorStatus> Statuses { get; set; } = new List<IndicatorStatus>();


        public object Clone()
        {
            KeyboardIndicator i = new KeyboardIndicator();

            i.Name = Name;
            i.ActiveColorHex = ActiveColorHex;
            i.IndicatorShape = IndicatorShape;
            i.Type = Type;

            for(int j = 0; j < Statuses.Count; j++)
            {
                i.Statuses.Add((IndicatorStatus)Statuses[j].Clone());
            }

            return i;
        }
        
    }
}
