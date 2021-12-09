using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class IndicatorStatus : ICloneable
    {

        public IndicatorStatus()
        {

        }

        public IndicatorStatus(int code, string text)
        {
            Code = code;
            Text = text;
        }

        public int Code { get; set; }

        public string Text { get; set; }

        public string Color { get; set; } = "#FF000000";

        public override string ToString()
        {
            return Text;
        }

        public object Clone()
        {
            IndicatorStatus s = new IndicatorStatus(Code, Text);
            s.Color = Color;

            return s;
        }

    }
}
