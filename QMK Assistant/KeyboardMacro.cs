using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class KeyboardMacro : ICloneable
    {

        public KeyboardMacro()
        {

        }

        public int Column { get; set; }

        public int Row { get; set; }

        public int Layer { get; set; }

        public string MacroText { get; set; }

        public MacroType MacroType { get; set; } = MacroType.Text;


        public object Clone()
        {
            KeyboardMacro m = new KeyboardMacro();
            m.Column = Column;
            m.Row = Row;
            m.Layer = Layer;
            m.MacroText = MacroText;

            return m;
        }

    }
}
