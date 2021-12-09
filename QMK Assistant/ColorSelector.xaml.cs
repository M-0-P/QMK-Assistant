using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QMK_Assistant
{
    /// <summary>
    /// Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : UserControl
    {
        public ColorSelector()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "OnValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<string>), typeof(ColorSelector));

        public event RoutedPropertyChangedEventHandler<string> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }

            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public static readonly DependencyProperty ColorHexProperty = DependencyProperty.Register("ColorHex", typeof(string), typeof(ColorSelector), new PropertyMetadata("FFFFFFFF", new PropertyChangedCallback(OnValueChanged)));

        public string ColorHex
        {
            get
            {

                return (string)GetValue(ColorHexProperty);
            }
            set
            {

                SetValue(ColorHexProperty, value);
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorSelector k = (ColorSelector)d;

            string oldvalue = (string)e.OldValue;
            string newvalue = (string)e.NewValue;

            RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(oldvalue, newvalue, ValueChangedEvent);

            k.OnValueChanged(args);

        }

        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<string> args)
        {
            RaiseEvent(args);
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            ;
            Color color = (Color)ColorConverter.ConvertFromString(ColorHex);
            System.Windows.Forms.ColorDialog c = new System.Windows.Forms.ColorDialog();
            c.Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

            if (c.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ColorHex = "#" + c.Color.A.ToString("X2") + c.Color.R.ToString("X2") + c.Color.G.ToString("X2") + c.Color.B.ToString("X2");
            }
        }
    }
}
