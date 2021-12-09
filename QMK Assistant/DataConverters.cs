using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QMK_Assistant
{
    public class ShapeToGeometry : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value.ToString();
            IndicatorShape i = Assistant.IndicatorShapes.Find(x => x.Name.Equals(name));

            return i.ShapePath;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }

    public class HexToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = value.ToString().Substring(1, value.ToString().Length - 1);
            Brush b;

            b = new SolidColorBrush(Color.FromArgb(
                byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));
            return b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush color = (SolidColorBrush)value;
            return color.ToString();
        }

    }
        public class UnitsToSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(value.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(value.ToString()) / QMK_Assistant.Properties.Settings.Default.BaseUnit;
        }

    }

    public class UnitsToBorder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(value.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.KeyBorderGap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double.Parse(value.ToString()) - QMK_Assistant.Properties.Settings.Default.KeyBorderGap) / QMK_Assistant.Properties.Settings.Default.BaseUnit;
        }

    }

    public class IndexEnable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int i = int.Parse(value.ToString());
            if(i == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

    public class ListboxItemSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            ListBox listBox = values[0] as ListBox;
            double width = listBox.ActualWidth / listBox.Items.Count;
            //Subtract 1, otherwise we could overflow to two rows.
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            TabControl tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / tabControl.Items.Count;
            //Subtract 1, otherwise we could overflow to two rows.
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }



    public class TextColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = value.ToString().Substring(1, value.ToString().Length - 1);

            double r = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            double g = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            double b = int.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            ColorBrightness bright = new ColorBrightness();
            bright = KeyboardFunctions.GetColorBrightness(r, g, b);

            if (bright == ColorBrightness.Light)
            {
                return Brushes.Black;
            }
            return Brushes.White;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }

    public class BoolVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility v = Visibility.Hidden;

            if ((bool)value)
            {
                v = Visibility.Visible;
            }

            return v;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }



    public class GridVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GridLength l = new GridLength(0,GridUnitType.Pixel);

            if ((bool)value)
            {
                l = new GridLength(1, GridUnitType.Auto);
            }

            return l;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }
    public class HexToBorder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = value.ToString().Substring(1, value.ToString().Length - 1);

            double factor = QMK_Assistant.Properties.Settings.Default.BorderShadeFactor;


            int a = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int r = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            if (KeyboardFunctions.GetTooDark(r, g, b) == ColorBrightness.Dark)
            {
                factor /= factor;
            }

            r = int.Parse(Math.Round(r * factor, 0).ToString());
            g = int.Parse(Math.Round(g * factor, 0).ToString());
            b = int.Parse(Math.Round(b * factor, 0).ToString());

            string h = "#" + a.ToString("X") + r.ToString("X") + g.ToString("X") + b.ToString("X");

            return h;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush color = (SolidColorBrush)value;
            return color.ToString();
        }

    }

    public class UnitsToCanvas : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(value.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double.Parse(value.ToString()) - QMK_Assistant.Properties.Settings.Default.LegendMargin) / QMK_Assistant.Properties.Settings.Default.BaseUnit;
        }

    }


    public class UnitsToCanvasBorder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double.Parse(value.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin) + 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double.Parse(value.ToString()) - QMK_Assistant.Properties.Settings.Default.LegendMargin + 2) / QMK_Assistant.Properties.Settings.Default.BaseUnit;
        }

    }


    public class LegendToCanvasW : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name =((LayerLegend)value).Name;
            KeyLegend i = Assistant.KeyLegends.Find(x => x.Name.Equals(name));

            return double.Parse(i.WidthU.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }

    public class LegendToCanvasH : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = ((LayerLegend)value).Name;
            KeyLegend i = Assistant.KeyLegends.Find(x => x.Name.Equals(name));

            return double.Parse(i.HeightU.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }

    public class LegendToGeometry : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = ((LayerLegend)value).Name;
            KeyLegend i = Assistant.KeyLegends.Find(x => x.Name.Equals(name));

            return i.LegendPath;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "";
        }

    }

    public class GetLegendWidth : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            List<LayerLegend> legends = (List<LayerLegend>)values[0];
            int i = (int)values[1];
            LayerLegend l = legends[i];
            KeyLegend kl = Assistant.KeyLegends.Find(x => x.Name.Equals(l.Name) && x.Group.Equals(l.Group));

            return double.Parse(kl.WidthU.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    public class IndicatorEnable : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            int i = (int)values[0];
            string t = values[1].ToString();

            if(i == -1 || t == "CAPS" || t == "Layer")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    public class GetLegendHeight : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            List<LayerLegend> legends = (List<LayerLegend>)values[0];
            int i = (int)values[1];
            LayerLegend l = legends[i];
            KeyLegend kl = Assistant.KeyLegends.Find(x => x.Name.Equals(l.Name) && x.Group.Equals(l.Group));

            return double.Parse(kl.HeightU.ToString()) * QMK_Assistant.Properties.Settings.Default.BaseUnit - QMK_Assistant.Properties.Settings.Default.LegendMargin;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }




}
