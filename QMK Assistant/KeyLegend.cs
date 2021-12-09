using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QMK_Assistant
{
    public class KeyLegend
    {
        public KeyLegend()
        {

        }

        public KeyLegend(string name, string height, string width, string legendpath)
        {
            Name = name;
            LegendPath = legendpath;
            HeightU = double.Parse(height);
            WidthU = double.Parse(width);
        }


        private string pathdata;

        public string Name { get; set; }

        public string Group { get; set; }

        public double HeightU { get; set; }

        public double WidthU { get; set; }

        public string LegendPath { get; set; }


        public string PathData
        {
            get
            {
                return pathdata;
            }
            set
            {
                pathdata = value;
            }
        }

        public Geometry PathGeometry
        {
            get
            {
                return Geometry.Parse(pathdata);
            }
            set
            {
                pathdata = value.ToString();
            }
        }


        public Geometry GetPathGeometry()
        {
            return Geometry.Parse(PathData);

        }


        public override string ToString()
        {
            return Group + "." + Name;
        }

        public bool Equals(string fullname)
        {
            string[] s = fullname.Split('.');

            if (s[0] == Group && s[1] == Name)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
