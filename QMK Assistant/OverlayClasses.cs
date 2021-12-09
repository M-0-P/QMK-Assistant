using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public class OverlaySize
    {
        public  OverlaySize()
        {
            
        }

        public OverlaySize(double ratio, string name)
        {
            Ratio = ratio;
            Name = name;
        }

        public double Ratio { get; set; }


        public string Name { get; set; }
    }

    public class OverlayPosition
    {
        public OverlayPosition()
        {

        }

        public OverlayPosition( string name, OverlayHorizontalAlignment halign, double hmargin, OverlayVerticalAlignment valign, double vmargin)
        {
            Name = name;
            HorizontalAlignment = halign;
            HorizontalMargin = hmargin;
            VerticalAlignment = valign;
            VerticalMargin = vmargin; 
        }

        public string Name { get; set; }

        public OverlayHorizontalAlignment HorizontalAlignment { get; set; }

        public double HorizontalMargin { get; set; }

        public OverlayVerticalAlignment VerticalAlignment { get; set; }

        public double VerticalMargin { get; set; }
    }
}
