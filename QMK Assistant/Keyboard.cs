using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class Keyboard : ICloneable
    {
        public Keyboard()
        {

        }

        public Keyboard( string name)
        {
            Name = name;
            for (int i = 0; i < QMK_Assistant.Properties.Settings.Default.LayerMax; i++)
            {
                KeyboardLayer l = new KeyboardLayer(i);
                Layers[i] = l;
            }

            QMKOpacityUp = Properties.Settings.Default.QMKDisplayOpacityUp;
            QMKOpacityDown = Properties.Settings.Default.QMKDisplayOpacityDown;
            QMKTypeUp = Properties.Settings.Default.QMKDisplayTypeUp;
            QMKTypeDown = Properties.Settings.Default.QMKDisplayTypeDown;
            QMKSizeUp = Properties.Settings.Default.QMKDisplaySizeUp;
            QMKSizeDown = Properties.Settings.Default.QMKDisplaySizeDown;
            QMKPositionUp = Properties.Settings.Default.QMKDisplayPositionUp;
            QMKPositionDown = Properties.Settings.Default.QMKDisplayPositionDown;
            QMKKeyboardUp = Properties.Settings.Default.QMKKeyboardUp;
            QMKKeyboardDown = Properties.Settings.Default.QMKKeyboardDown;
            QMKMonitor = Properties.Settings.Default.QMKMonitor;
            QMKSave = Properties.Settings.Default.QMKDisplaySettingsSave;
            QMKStringPrefix = Properties.Settings.Default.DefaultQMKPrefix;
            QMKStringSuffix = Properties.Settings.Default.DefaultQMKSuffix;
            QMKLayerCode = Properties.Settings.Default.DefaultQMKLayer;
            QMKKeystrokeCode = Properties.Settings.Default.DefaultQMKKeystroke;
            QMKMacroCode = Properties.Settings.Default.DefaultQMKMacro;
            QMKCapsCode = Properties.Settings.Default.DefaultQMKCaps;
            QMKIndicatorCode = Properties.Settings.Default.DefaultQMKIndicator;
            QMKQMKKeyCode = Properties.Settings.Default.DefaultQMKQMK;
        }

        public Keyboard(string name, double width, double height, string color, string vendor, string product, string version,            
                        string opacityup, string opacitydown, string typeup, string typedown, string sizeup, string sizedown, 
                        string positionup, string positiondown, string keyboardup, string keyboarddown, string monitor,
                        string save, string prefix, string suffix, string layer, string keycode, string macro, string caps, string indicator, string qmk)
        {
            Name = name;
            WidthU = width;
            HeightU = height;
            KeyColor = color;
            VendorId = vendor;
            ProductId = product;
            Version = version;

            QMKOpacityUp = opacityup;
            QMKOpacityDown = opacitydown;
            QMKTypeUp = typeup;
            QMKTypeDown = typedown;
            QMKSizeUp = sizeup;
            QMKSizeDown = sizedown;
            QMKPositionUp = positionup;
            QMKPositionDown = positiondown;
            QMKKeyboardUp = keyboardup;
            QMKKeyboardDown = keyboarddown;
            QMKMonitor = monitor;
            QMKSave = save;
            QMKStringPrefix = prefix;
            QMKStringSuffix = suffix;
            QMKLayerCode = layer;
            QMKKeystrokeCode = keycode;
            QMKMacroCode = macro;
            QMKCapsCode = caps;
            QMKIndicatorCode = indicator;
            QMKQMKKeyCode = qmk;
        }

        public Keyboard(string name, Keyboard k)
        {
            Name = name;
            WidthU = k.WidthU;
            HeightU = k.HeightU;
            KeyColor = k.KeyColor;
            VendorId = k.VendorId;
            ProductId = k.ProductId;
            Version = k.Version;

            for(int i = 0; i < k.Layers.Length; i++)
            {
                Layers[i] = (KeyboardLayer)k.Layers[i].Clone();
            }

            for (int i = 0; i < k.Keys.Count; i++)
            {
                Keys.Add((KeyboardKey)k.Keys[i].Clone());
            }

            for (int i = 0; i < k.Macros.Count; i++)
            {
                Macros.Add((KeyboardMacro)k.Macros[i].Clone());
            }

            for (int i = 0; i < k.Indicators.Count; i++)
            {
                Indicators.Add((KeyboardIndicator)k.Indicators[i].Clone());
            }


            QMKOpacityUp = k.QMKOpacityUp;
            QMKOpacityDown = k.QMKOpacityDown;
            QMKTypeUp = k.QMKTypeUp;
            QMKTypeDown = k.QMKTypeDown;
            QMKSizeUp = k.QMKSizeUp;
            QMKSizeDown = k.QMKSizeDown;
            QMKPositionUp = k.QMKPositionUp;
            QMKPositionDown = k.QMKPositionDown;
            QMKKeyboardUp = k.QMKKeyboardUp;
            QMKKeyboardDown = k.QMKKeyboardDown;
            QMKMonitor = k.QMKMonitor;
            QMKSave = k.QMKSave;
            QMKStringPrefix = k.QMKStringPrefix;
            QMKStringSuffix = k.QMKStringSuffix;
            QMKLayerCode = k.QMKLayerCode;
            QMKKeystrokeCode = k.QMKKeystrokeCode;
            QMKMacroCode = k.QMKMacroCode;
            QMKCapsCode = k.QMKCapsCode;
            QMKIndicatorCode = k.QMKIndicatorCode;
            QMKQMKKeyCode = k.QMKQMKKeyCode;
        }


        public Keyboard(string[] data)
        {
            Name = data[1];
            WidthU = double.Parse(data[2]);
            HeightU = double.Parse(data[3]);
            KeyColor = data[4];
            VendorId = data[5];
            ProductId = data[6];
            Version = data[7];



            QMKOpacityUp = data[8];
            QMKOpacityDown = data[9];
            QMKTypeUp = data[10];
            QMKTypeDown = data[11];
            QMKSizeUp = data[12];
            QMKSizeDown = data[13];
            QMKPositionUp = data[14];
            QMKPositionDown = data[15];
            QMKKeyboardUp = data[16];
            QMKKeyboardDown = data[17];
            QMKMonitor = data[18];
            QMKSave = data[19];
            QMKStringPrefix = data[20];
            QMKStringSuffix = data[21];
            QMKLayerCode = data[22];
            QMKKeystrokeCode = data[23];
            QMKMacroCode = data[24];
            QMKCapsCode = data[25];
            QMKIndicatorCode = data[26];
            QMKQMKKeyCode = data[27];
        }
        public string Name { get; set; }

        public string VendorId { get; set; }

        public string ProductId { get; set; }

        public string Version { get; set; }

        public double HeightU { get; set; } = 5;

        public double WidthU { get; set; } = 13;

        public string KeyColor { get; set; } = "#FFFFFFFF";

        public KeyboardLayer[] Layers = new KeyboardLayer[QMK_Assistant.Properties.Settings.Default.LayerMax];
    

        public List<KeyboardKey> Keys { get; set; } = new List<KeyboardKey>();

        public List<KeyboardMacro> Macros { get; set; } = new List<KeyboardMacro>();

        public List<KeyboardIndicator> Indicators { get; set; } = new List<KeyboardIndicator>();


        public string GetDeviceProperties()
        {
            return VendorId + ":" + ProductId + ":" + Version;
        }

        public override string ToString()
        {
            return Name;
        }

        public string QMKOpacityUp { get; set; }

        public string QMKOpacityDown { get; set; }

        public string QMKTypeUp { get; set; }

        public string QMKTypeDown { get; set; }

        public string QMKSizeUp { get; set; }

        public string QMKSizeDown { get; set; }

        public string QMKPositionUp { get; set; }

        public string QMKPositionDown { get; set; }

        public string QMKKeyboardUp { get; set; }

        public string QMKKeyboardDown { get; set; }

        public string QMKMonitor { get; set; }

        public string QMKSave { get; set; }

        public string QMKStringPrefix { get; set; }

        public string QMKStringSuffix { get; set; }

        public string QMKLayerCode { get; set; }

        public string QMKKeystrokeCode { get; set; }

        public string QMKMacroCode { get; set; }

        public string QMKCapsCode { get; set; }
        public string QMKIndicatorCode { get; set; }

        public string QMKQMKKeyCode { get; set; }

        public string GetSaveLine()
        {
          return "Keyboard" + "," + Name + "," + WidthU.ToString() + "," + HeightU.ToString() + "," + KeyColor + "," + VendorId + "," + ProductId + "," + Version
                        + "," + QMKOpacityUp + "," + QMKOpacityDown + "," + QMKTypeUp + "," + QMKTypeDown + "," + QMKSizeUp + "," + QMKSizeDown
                        + "," + QMKPositionUp + "," + QMKPositionDown + "," + QMKKeyboardUp + "," + QMKKeyboardDown + "," + QMKMonitor
                        + "," + QMKSave + "," + QMKStringPrefix + "," + QMKStringSuffix + "," + QMKLayerCode + "," + QMKKeystrokeCode + "," + QMKMacroCode + "," + QMKCapsCode + "," + QMKIndicatorCode + "," + QMKQMKKeyCode;
        }
        public object Clone()
        {
            return new Keyboard(Name, this);
        }
    }
}
