using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QMK_Assistant
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {

        #region Transparecy
        public OverlayWindow()
        {
            InitializeComponent();
            this.Topmost = true;

            Thread globalMouseListener = new Thread(() =>
            {
                while (true)
                {
                    Point p1 = GetMousePosition();
                    bool mouseInControl = false;



                    Thread.Sleep(15);
                }
            });

            globalMouseListener.Start();
        }

        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);
        public const uint WS_EX_LAYERED = 0x00080000;

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        public static void SetWindowExNotTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowExTransparent(hwnd);
        }

        #endregion
        private Keyboard activekeyboard;
        private Brush keybrush;
        private Brush borderBrush;
        private Brush capsoffBrush;
        private Brush capsonBrush;
        private SolidColorBrush selectedborderbrush;

        private int haslayerindicator = -1;
        private int hascapsindicator = -1;

        private List<Canvas> LayerCanvases = new List<Canvas>();


        private OverlayType overlayType = OverlayType.Both;
        private OverlaySize overlaySize = new OverlaySize(1,"Full");
        private OverlayPosition overlayPosition = new OverlayPosition("Center", OverlayHorizontalAlignment.Center,0, OverlayVerticalAlignment.Middle,0);
        private double overlayOpacity;

        BrushConverter converter = new System.Windows.Media.BrushConverter();

        double corner = 4;
        double gap = QMK_Assistant.Properties.Settings.Default.KeyBorderGap / 2;
        double units = QMK_Assistant.Properties.Settings.Default.BaseUnit;
        double insidemargin = QMK_Assistant.Properties.Settings.Default.LegendMargin / 2;
        double legendunit = QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin;

        public void ActivateKeyboard(Keyboard keyboard)
        {
            activekeyboard = keyboard;

            if(activekeyboard == null)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
                UpdateData();
            }
            

        }



        private void UpdateData()
        {
            UpdateKeyBrushes();
            UpdateKeyCanvases();
            UpdateLegendCanvases();
            UpdateIndicatorCanvas();
            UpdateOverlayDisplay();
            GetCapsInfo();
            GetLayerId();
            InitializeStatus();
        }

        public void InitializeStatus()
        {
            UpdateLayer("0");
            UpdateCaps("0");
        }

        public void UpdateOverlayType(OverlayType type)
        {
            overlayType = type;
            UpdateOverlayDisplay();
        }


        public void UpdateOverlaySize(OverlaySize size)
        {
            overlaySize = size;
            UpdateOverlayDisplay();
        }

        public void UpdateOverlayPosition(OverlayPosition position)
        {
            overlayPosition = position;
            UpdateOverlayDisplay();
        }

        public void UpdateOverlayOpacity(double opacity)
        {
            overlayOpacity = opacity;
            UpdateOverlayDisplay();
        }


        private void UpdateOverlayDisplay()
        {
            double wWA = System.Windows.SystemParameters.WorkArea.Width;
            double hWA = System.Windows.SystemParameters.WorkArea.Height;
            double xWA = System.Windows.SystemParameters.WorkArea.X;
            double yWA = System.Windows.SystemParameters.WorkArea.Y;
            double keyboardHeight = 0;
            double keyboardWidth = 0;
            double indicatorHeight = 0;
            double indicatorWidth = 0;
            double sizemultiple = 1;
            double topoffset = 0;
            double leftoffset = 0;




            switch (overlayType)
            {
                case OverlayType.Both:
                    indicatorHeight = 1;
                    keyboardHeight = activekeyboard.HeightU;
                    indicatorWidth = activekeyboard.Indicators.Count;
                    keyboardWidth = activekeyboard.WidthU;
                    break;
                case OverlayType.Indicator:
                    indicatorHeight = 1;
                    keyboardHeight = 0;
                    indicatorWidth = activekeyboard.Indicators.Count;
                    keyboardWidth = 0;
                    break;
                case OverlayType.Keyboard:
                    indicatorHeight = 0;
                    keyboardHeight = activekeyboard.HeightU;
                    indicatorWidth = 0;
                    keyboardWidth = activekeyboard.WidthU;
                    break;
                case OverlayType.None:
                    indicatorHeight = 0;
                    keyboardHeight = 0;
                    indicatorWidth = 0;
                    keyboardWidth = 0;
                    break;
            }

            sizemultiple = overlaySize.Ratio;

            double x = Math.Min(sizemultiple * wWA / (Properties.Settings.Default.BaseUnit * Math.Max(indicatorWidth,keyboardWidth)), sizemultiple * hWA / (Properties.Settings.Default.BaseUnit * (indicatorHeight + keyboardHeight)));

            IndicatorRow.Height = new GridLength(indicatorHeight, GridUnitType.Star);
            KeyboardRow.Height = new GridLength(keyboardHeight, GridUnitType.Star);

            this.Width = Properties.Settings.Default.BaseUnit * x * Math.Max(indicatorWidth, keyboardWidth);
            this.Height = Properties.Settings.Default.BaseUnit * x * (indicatorHeight + keyboardHeight);
            IndicatorCanvas.Width = Properties.Settings.Default.BaseUnit * indicatorWidth;
            IndicatorCanvas.Height = Properties.Settings.Default.BaseUnit * indicatorHeight;
            LayoutCanvas.Width = Properties.Settings.Default.BaseUnit * activekeyboard.WidthU;
            LayoutCanvas.Height = Properties.Settings.Default.BaseUnit * activekeyboard.HeightU;
            this.Opacity = overlayOpacity;


            switch(overlayPosition.HorizontalAlignment)
            {
                case OverlayHorizontalAlignment.Left:
                    leftoffset = xWA + overlayPosition.HorizontalMargin;
                    break;
                case OverlayHorizontalAlignment.Right:
                    leftoffset = xWA + wWA - this.Width - overlayPosition.HorizontalMargin;
                    break;
                case OverlayHorizontalAlignment.Center:
                    leftoffset = xWA + wWA / 2 - this.Width / 2;
                    break;
            }
            switch (overlayPosition.VerticalAlignment)
            {
                case OverlayVerticalAlignment.Top:
                    topoffset = yWA + overlayPosition.VerticalMargin;
                    break;
                case OverlayVerticalAlignment.Bottom:
                    topoffset = yWA + hWA - this.Height - overlayPosition.VerticalMargin;
                    break;
                case OverlayVerticalAlignment.Middle:
                    topoffset = yWA + hWA / 2 - this.Height / 2;
                    break;
            }

            this.Top = topoffset;
            this.Left = leftoffset;

            UpdateWindowVisibility();

        }

        private void UpdateKeyBrushes()
        {
            string color = activekeyboard.KeyColor.ToString().Substring(1, activekeyboard.KeyColor.ToString().Length - 1);

            double factor = QMK_Assistant.Properties.Settings.Default.BorderShadeFactor;


            int a = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int r = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);



            if (KeyboardFunctions.GetTooDark(r, g, b) == ColorBrightness.Dark)
            {
                factor = 1 / factor;
                selectedborderbrush = Brushes.White;
            }
            else
            {
                selectedborderbrush = Brushes.Black;
            }

            r = int.Parse(Math.Round(r * factor, 0).ToString());
            g = int.Parse(Math.Round(g * factor, 0).ToString());
            b = int.Parse(Math.Round(b * factor, 0).ToString());

            string h = "#" + a.ToString("X") + r.ToString("X") + g.ToString("X") + b.ToString("X");

            

            keybrush = (Brush)converter.ConvertFromString(activekeyboard.KeyColor);
            borderBrush = (Brush)converter.ConvertFromString(h);
        }

        private void UpdateWindowVisibility()
        {


            if (overlayType == OverlayType.None)
            {

                if (this.Visibility != Visibility.Hidden)
                {
                    this.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                if (this.Visibility != Visibility.Visible)
                {
                    this.Visibility = Visibility.Visible;
                }


            }
        }

        private void UpdateKeyCanvases()
        {
            LayoutCanvas.Children.Clear();
            Canvas outlinecanvas = new Canvas();
            Canvas keycanvas = new Canvas();
            foreach (KeyboardKey k in activekeyboard.Keys)
            {
                System.Windows.Shapes.Path outline = new System.Windows.Shapes.Path();

                KeyboardGridLocation g = new KeyboardGridLocation(k.Column, k.Row);
                outline.Fill = borderBrush;
                outline.Tag = g;

                StreamGeometry outlinegeometry = new StreamGeometry();

                // a  b
                //h    c
                //g    d
                // f  e
                using (StreamGeometryContext context = outlinegeometry.Open())
                {

                    Point tlbp = new Point(k.XU * units + gap, k.YU * units + gap);
                    Point trbp = new Point(k.XU * units + k.WidthU * units - gap, k.YU * units + gap);
                    Point brbp = new Point(k.XU * units + k.WidthU * units - gap, k.YU * units + k.HeightU * units - gap);
                    Point blbp = new Point(k.XU * units + gap, k.YU * units + k.HeightU * units - gap);
                    Point pa = new Point(k.XU * units + corner + gap, k.YU * units + gap);
                    Point pb = new Point(k.XU * units + k.WidthU * units - corner - gap, k.YU * units + gap);
                    Point pc = new Point(k.XU * units + k.WidthU * units - gap, k.YU * units + corner + gap);
                    Point pd = new Point(k.XU * units + k.WidthU * units - gap, k.YU * units + k.HeightU * units - corner - gap);
                    Point pe = new Point(k.XU * units + k.WidthU * units - corner - gap, k.YU * units + k.HeightU * units - gap);
                    Point pf = new Point(k.XU * units + corner + gap, k.YU * units + k.HeightU * units - gap);
                    Point pg = new Point(k.XU * units + gap, k.YU * units + k.HeightU * units - corner - gap);
                    Point ph = new Point(k.XU * units + gap, k.YU * units + corner + gap);

                    context.BeginFigure(pa, true, true);
                    context.LineTo(pb, true, true);
                    context.BezierTo(trbp, trbp, pc, true, true);
                    context.LineTo(pd, true, true);
                    context.BezierTo(brbp, brbp, pe, true, true);
                    context.LineTo(pf, true, true);
                    context.BezierTo(blbp, blbp, pg, true, true);
                    context.LineTo(ph, true, true);
                    context.BezierTo(tlbp, tlbp, pa, true, true);
                }

                outlinegeometry.Freeze();
                outline.Data = outlinegeometry;
                outlinecanvas.Children.Add(outline);

                System.Windows.Shapes.Path inside = new System.Windows.Shapes.Path();

                inside.Fill = keybrush;

                StreamGeometry insidegeometry = new StreamGeometry();


                using (StreamGeometryContext insidecontext = insidegeometry.Open())
                {

                    Point tlbp = new Point(k.XU * units + insidemargin, k.YU * units + insidemargin);
                    Point trbp = new Point(k.XU * units + k.WidthU * units - insidemargin, k.YU * units + insidemargin);
                    Point brbp = new Point(k.XU * units + k.WidthU * units - insidemargin, k.YU * units + k.HeightU * units - insidemargin - gap);
                    Point blbp = new Point(k.XU * units + insidemargin, k.YU * units + k.HeightU * units - insidemargin);
                    Point pa = new Point(k.XU * units + corner + insidemargin, k.YU * units + insidemargin);
                    Point pb = new Point(k.XU * units + k.WidthU * units - corner - insidemargin, k.YU * units + insidemargin);
                    Point pc = new Point(k.XU * units + k.WidthU * units - insidemargin, k.YU * units + corner + insidemargin);
                    Point pd = new Point(k.XU * units + k.WidthU * units - insidemargin, k.YU * units + k.HeightU * units - insidemargin - corner);
                    Point pe = new Point(k.XU * units + k.WidthU * units - insidemargin - corner, k.YU * units + k.HeightU * units - insidemargin);
                    Point pf = new Point(k.XU * units + corner + insidemargin, k.YU * units + k.HeightU * units - insidemargin);
                    Point pg = new Point(k.XU * units + insidemargin, k.YU * units + k.HeightU * units - insidemargin - corner);
                    Point ph = new Point(k.XU * units + insidemargin, k.YU * units + corner + insidemargin);

                    insidecontext.BeginFigure(pa, true, true);
                    insidecontext.LineTo(pb, true, true);
                    insidecontext.BezierTo(trbp, trbp, pc, true, true);
                    insidecontext.LineTo(pd, true, true);
                    insidecontext.BezierTo(brbp, brbp, pe, true, true);
                    insidecontext.LineTo(pf, true, true);
                    insidecontext.BezierTo(blbp, blbp, pg, true, true);
                    insidecontext.LineTo(ph, true, true);
                    insidecontext.BezierTo(tlbp, tlbp, pa, true, true);
                }
                insidegeometry.Freeze();
                inside.Data = insidegeometry;
                keycanvas.Children.Add(inside);


            }

            LayoutCanvas.Children.Add(outlinecanvas);
            LayoutCanvas.Children.Add(keycanvas);

        }

        private void UpdateLegendCanvases()
        {
            LayerCanvases.Clear();
            BrushConverter converter = new BrushConverter();

            foreach (KeyboardLayer l in activekeyboard.Layers)
            {
                Canvas c = new Canvas();
                foreach (KeyboardKey k in activekeyboard.Keys)
                {
                    System.Windows.Shapes.Path legend = new System.Windows.Shapes.Path();
                    KeyLegend keyLegend = Assistant.KeyLegends.Find(x => x.Name.Equals(k.Legends[l.Priority].Name) && x.Group.Equals(k.Legends[l.Priority].Group));

                    if(keyLegend ==null)
                    {
                        keyLegend = Assistant.KeyLegends.Find(x => x.Name.Equals("(Blank)") && x.Group.Equals("(Blank)"));
                    }
                    legend.StrokeThickness = 0.25;
                    legend.Fill = (Brush)converter.ConvertFromString(l.ColorHex);
                    legend.Fill.Freeze();
                    Geometry legdata;


                    legdata = Geometry.Parse(keyLegend.LegendPath);


                    TranslateTransform t = new TranslateTransform((k.XU + .5 * k.WidthU) * units - (keyLegend.WidthU * legendunit) / 2, ((k.YU + .5 * k.HeightU) * units - (keyLegend.HeightU * legendunit) / 2));


                    Geometry trns = Geometry.Combine(legdata, legdata, GeometryCombineMode.Intersect, t);

                    legend.Data = trns;
                    legend.Data.Freeze();

                    c.Children.Add(legend);
                }
                LayerCanvases.Add(c);

            }
        }

        private void UpdateIndicatorCanvas()
        {
            IndicatorCanvas.Children.Clear();
            Canvas indicatorCanvas = new Canvas();
            for( int i = 0; i < activekeyboard.Indicators.Count; i ++)
            {

                KeyboardIndicator ki = activekeyboard.Indicators[i];
                System.Windows.Shapes.Path indicator = new System.Windows.Shapes.Path();
                //indicator.Name = ki.Code.ToString();
                indicator.Tag = ki.Type;
                indicator.Fill = Brushes.Black;
                string shape = Assistant.IndicatorShapes.Find(x => x.Name == ki.IndicatorShape).ShapePath;

                Geometry indicatordata = Geometry.Parse(shape);

                TranslateTransform t = new TranslateTransform(i * units + insidemargin, insidemargin);

                Geometry trns = Geometry.Combine(indicatordata, indicatordata, GeometryCombineMode.Intersect, t);

                indicator.Data = trns;
                indicator.Data.Freeze();

                indicatorCanvas.Children.Add(indicator);

            }

            IndicatorCanvas.Children.Add(indicatorCanvas);
        }

        private void GetCapsInfo()
        {

            hascapsindicator = activekeyboard.Indicators.FindIndex(x => x.Type == IndicatorType.CAPS);

            if(hascapsindicator > -1)
            {
                capsoffBrush = (Brush)converter.ConvertFromString(activekeyboard.Indicators[hascapsindicator].Statuses.Find(x => x.Text == "Off").Color);
                capsonBrush = (Brush)converter.ConvertFromString(activekeyboard.Indicators[hascapsindicator].Statuses.Find(x => x.Text == "On").Color);
            }
            
        }

        private void GetLayerId()
        {
            haslayerindicator = -1;
            for (int i = 0; i < ((Canvas)IndicatorCanvas.Children[0]).Children.Count; i++)
            {

                Path p = (Path)((Canvas)IndicatorCanvas.Children[0]).Children[i];
                if (p.Tag.ToString() == "Layer")
                {
                    haslayerindicator = i;
                }
            }
        }
        public void UpdateLayer(string layer)
        {

            int l = int.Parse(layer);

            if (LayoutCanvas.Children.Count > 2)
            {
                LayoutCanvas.Children.RemoveAt(2);
            }
            LayoutCanvas.Background = (Brush)converter.ConvertFromString(activekeyboard.Layers[l].ColorHex);
            LayoutCanvas.Children.Insert(2, LayerCanvases[l]);


            if(haslayerindicator >-1)
            {
                ((Path)((Canvas)IndicatorCanvas.Children[0]).Children[haslayerindicator]).Fill = (Brush)converter.ConvertFromString(activekeyboard.Layers[l].ColorHex);
            }
           
        }

        public void UpdateIndicator(string data)
        {
            int indicator = int.Parse(data.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
            int status = int.Parse(data.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);

            int i = activekeyboard.Indicators.FindIndex(x => x.Code == indicator);

            Brush c = (Brush)converter.ConvertFromString(activekeyboard.Indicators[i].Statuses.Find(x => x.Code == status).Color);

            ((Path)((Canvas)IndicatorCanvas.Children[0]).Children[i]).Fill = c;
        }
        public void UpdateCaps(string status)
        {
            if(hascapsindicator >-1)
            {
                bool on = Convert.ToBoolean(int.Parse(status));

                if (on)
                {
                    ((Path)((Canvas)IndicatorCanvas.Children[0]).Children[hascapsindicator]).Fill = capsonBrush;
                }
                else
                {
                    ((Path)((Canvas)IndicatorCanvas.Children[0]).Children[hascapsindicator]).Fill = capsoffBrush;
                }
            }

        }

        public void UpdateKeyPress(string data)
        {
            int column = Convert.ToInt32(data.Substring(0, 2), 16);
            int row = Convert.ToInt32(data.Substring(2, 2), 16);
            bool pressed = Convert.ToBoolean(int.Parse(data.Substring(4, 1)));

            KeyboardGridLocation g = new KeyboardGridLocation(column, row);

             if(LayoutCanvas.Children.Count > 0)
            { 

                foreach (Path p in ((Canvas)LayoutCanvas.Children[0]).Children)
                {
                    object x = p.Tag;
                    if (x != null)
                    {
                        if (x.Equals(g))
                        {
                            if(pressed)
                            {
                                p.Fill = selectedborderbrush;
                            }
                            else
                            {
                                p.Fill = borderBrush;
                            }
                        
                        }


                    }
                }
            }
        }

    }
}
