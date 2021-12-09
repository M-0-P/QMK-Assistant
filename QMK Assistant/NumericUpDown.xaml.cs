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
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();


            NUDTextBox.Text = string.Format(StringFormat(), StartValue);
            Value = StartValue;
        }




        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "OnValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double>), typeof(NumericUpDown));

        public event RoutedPropertyChangedEventHandler<double> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }

            remove { RemoveHandler(ValueChangedEvent, value); }
        }





        public double StartValue { get; set; } = 0;
        private double FocusValue;





        public double Value
        {
            get
            {
                
                return (double)GetValue(ValueProperty);
            }
            set
            {

                SetValue(ValueProperty, value);
            }
        }

        public int DecimalPlaces
        {
            get
            {

                return (int)GetValue(DecimalPlacesProperty);
            }
            set
            {

                SetValue(DecimalPlacesProperty, value);
            }
        }

        public double MinimumValue
        {
            get
            {

                return (double)GetValue(MinimumValueProperty);
            }
            set
            {

                SetValue(MinimumValueProperty, value);
            }
        }

        public double MaximumValue
        {
            get
            {

                return (double)GetValue(MaximumValueProperty);
            }
            set
            {

                SetValue(MaximumValueProperty, value);
            }
        }

        public double Increments
        {
            get
            {

                return (double)GetValue(IncrementsProperty);
            }
            set
            {

                SetValue(IncrementsProperty, value);
            }
        }

        public bool ForceIncrements
        {
            get
            {

                return (bool)GetValue(ForceIncrementsProperty);
            }
            set
            {

                SetValue(ForceIncrementsProperty, value);
            }
        }


        private string StringFormat()
        {
            string f = "";
            if (DecimalPlaces > 0)
            {
                f = ".";
                for (int i = 1; i <= DecimalPlaces; i++)
                {
                    f += "0";
                }
            }

            return "{0:0" + f + "}";
        }

        private double RoundInput(double value)
        {
            return Math.Round(value, DecimalPlaces);
        }

        public const double defaultvalue = 0;
        public const double defaultminvalue = 0;
        public const double defaultmaxvalue = 255;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new PropertyMetadata(defaultvalue, new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(NumericUpDown), new PropertyMetadata(QMK_Assistant.Properties.Settings.Default.UpDownDefaultDecimal));

        public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.Register("MinimumValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(defaultminvalue));

        public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.Register("MaximumValue", typeof(double), typeof(NumericUpDown), new PropertyMetadata(defaultmaxvalue));

        public static readonly DependencyProperty IncrementsProperty = DependencyProperty.Register("Increments", typeof(double), typeof(NumericUpDown), new PropertyMetadata(QMK_Assistant.Properties.Settings.Default.DefaultSpacing));

        public static readonly DependencyProperty ForceIncrementsProperty = DependencyProperty.Register("ForceIncrements", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(QMK_Assistant.Properties.Settings.Default.UpDownForceIncrements));

         
        public void UpdateText()
        {
            NUDTextBox.Text = string.Format(StringFormat(), Value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown k = (NumericUpDown)d;

            double oldvalue = (double)e.OldValue;
            double newvalue = (double)e.NewValue;

            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(oldvalue, newvalue, ValueChangedEvent);
            k.OnValueChanged(args);
            k.UpdateText();

        }


        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<double> args)
        {
            RaiseEvent(args);
        }


        private void NUDTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double x = double.NaN;
            try
            {
                x = RoundInput((double.Parse(NUDTextBox.Text)));
            }
            catch
            {
                MessageBox.Show("Enter valid number", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Value = FocusValue;
                UpdateText();
                return;
            }


            if ((x * Math.Pow(10,DecimalPlaces)) % (Increments * Math.Pow(10, DecimalPlaces)) != 0)
            {
                MessageBox.Show("Enter increments of " + Increments.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Value = FocusValue;
                UpdateText();
            }
            else if (x > MaximumValue || x < MinimumValue)
            {
                MessageBox.Show("Values can only be between " + StartValue + " and " + MaximumValue + ".", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Value = FocusValue;
                UpdateText();
            }
            else
            {
                Value = x;
            }
        }

        private void NUDTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FocusValue = double.Parse(NUDTextBox.Text);
        }

        private void NUDButtonUp_Click(object sender, RoutedEventArgs e)
        {
            double x = Value;
            x += Increments;
            if (x <= MaximumValue)
            {
                Value = x;
            }
            else
            {
                Value = MaximumValue;
            }
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            double x = Value;
            x -= Increments;
            if (x >= MinimumValue)
            {
                Value = x;
            }
            else
            {
                Value = MinimumValue;
            }
        }


    }
}
