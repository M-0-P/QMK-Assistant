using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class SaveKeyboardsEventArgs : EventArgs
    {
        public SaveKeyboardsEventArgs(List<Keyboard> keyboards)
        {
            Keyboards = keyboards;
        }

        public List<Keyboard> Keyboards { get; set; } = new List<Keyboard>();
    }
}
