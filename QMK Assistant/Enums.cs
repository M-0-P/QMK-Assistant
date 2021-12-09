using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{

    public enum ColorBrightness
    {
        Light,
        Dark
    }

    public enum Mods
    {
        Shift,
        Control,
        Alt,
        OEM
    }

    public enum IndicatorType
    {
        CAPS,
        Layer,
        OSMods,
        Custom
    }

    public enum OverlayType
    {
        None,
        Keyboard,
        Indicator,
        Both
    }


    public enum MacroType
    {
        KeyPress,
        Text
    }


    public enum OverlayVerticalAlignment
    {
        Middle,
        Top,
        Bottom
    }

    public enum OverlayHorizontalAlignment
    {
        Center,
        Left,
        Right
    }
}
