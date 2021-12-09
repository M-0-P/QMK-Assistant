using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class KeyboardKey : ICloneable
    {

        public KeyboardKey()
        {
        }

        private int rotation = 0;

        public KeyboardKey(int activelayer)
        {
            ActiveLayerId = activelayer;

            for (int i = 0; i < QMK_Assistant.Properties.Settings.Default.LayerMax; i++)
            {
                Legends[i] = new LayerLegend(i, "(Blank)", "(Blank)");
            }
        }

        public double RotationWidth
        {
            get
            {
                return WidthU * Math.Abs(Math.Cos(rotation * 2 * Math.PI / 360)) + HeightU * Math.Abs(Math.Sin(rotation * 2 * Math.PI / 360));
            }
        }


        public double RotationHeight
        {
            get
            {
                return WidthU * Math.Abs(Math.Sin(rotation * 2 * Math.PI / 360)) + HeightU * Math.Abs(Math.Cos(rotation * 2 * Math.PI / 360));
            }
        }

        //public string[] Legends = new string[QMK_Assistant.Properties.Settings.Default.LayerMax];

        public LayerLegend[] Legends = new LayerLegend[QMK_Assistant.Properties.Settings.Default.LayerMax];

        public int Row { get; set; }

        public int Column { get; set; }

        public double HeightU { get; set; } = 1;

        public double WidthU { get; set; } = 1;

        public double XU { get; set; } = 0;

        public double YU { get; set; } = 0;


        public int Rotation
        {
            get { return rotation; }
            set { rotation = KeyboardFunctions.Modulo(value, 360); }
        }

        public int ActiveLayerId { get; set; } = 0;


        public LayerLegend ActiveLegend
        {
            get { return Legends[ActiveLayerId]; }
            set { Legends[ActiveLayerId] = value; }
        }

        public object Clone()
        {
            KeyboardKey k = new KeyboardKey(ActiveLayerId);
            k.Column = Column;
            k.HeightU = HeightU;
            k.Rotation = Rotation;
            k.Row = Row;
            k.WidthU = WidthU;
            k.XU = XU;
            k.YU = YU;

            for(int i = 0; i < Legends.Length; i++)
            {
                k.Legends[i] = (LayerLegend)Legends[i].Clone();
            }

            return k;
        }
    }
}
