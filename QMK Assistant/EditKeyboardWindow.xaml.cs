using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace QMK_Assistant
{
    /// <summary>
    /// Interaction logic for EditKeyboardWindow.xaml
    /// </summary>
    public partial class EditKeyboardWindow : Window
    {
        private List<Keyboard> keyboards = new List<Keyboard>();

        bool needsave = false;

        //public EditKeyboardWindow()
        //{
        //    InitializeComponent();
        //    foreach(Keyboard k in Assistant.Keyboards)
        //    {
        //        keyboards.Add((Keyboard)k.Clone());
        //    }

        //    KeyboardComboBox.ItemsSource = keyboards;

        //    IndicatorShapeComboBox.ItemsSource = Assistant.IndicatorShapes;

        //}

        public EditKeyboardWindow(List<Keyboard> kbs)
        {
            InitializeComponent();
            foreach (Keyboard k in kbs)
            {
                keyboards.Add((Keyboard)k.Clone());
            }

            KeyboardComboBox.ItemsSource = keyboards;

            IndicatorShapeComboBox.ItemsSource = Assistant.IndicatorShapes;

        }




        private void KeyboardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KeyboardComboBox.SelectedIndex > -1)
            {
                LayerListBox.ItemsSource = ((Keyboard)KeyboardComboBox.SelectedItem).Layers;
                LayerListBox.SelectedIndex = 0;
                LayoutListView.ItemsSource = ((Keyboard)KeyboardComboBox.SelectedItem).Keys;
                IndicatorListBox.ItemsSource = ((Keyboard)KeyboardComboBox.SelectedItem).Indicators;
                IndicatorListBox.SelectedIndex = 0;
                MacroDataGrid.ItemsSource = ((Keyboard)KeyboardComboBox.SelectedItem).Macros;
                MacroDataGrid.Items.Refresh();
            }

        }

        private void LayoutListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //for( int i= 0; i < ((KeyboardKey)((ListView)sender).SelectedItem).Legends.Count(); i++)
            //{
            //    ((KeyboardKey)((ListView)sender).SelectedItem).LegendsTest[i] = new LayerLegend(i, ((KeyboardKey)((ListView)sender).SelectedItem).Legends[i]);
            //}
            if (((ListView)sender).SelectedIndex > -1)
            {
                LegendDataGrid.ItemsSource = ((KeyboardKey)((ListView)sender).SelectedItem).Legends;
            }
            else
            {
                LegendDataGrid.ItemsSource = null;
            }
        }

        private void LegendRowButton_Click(object sender, RoutedEventArgs e)
        {

            LegendViewWindow n = new LegendViewWindow((LayerLegend)LegendDataGrid.SelectedItems[0]);


            if (n.ShowDialog() == true && n.Answer != null)
            {
                
                for (int j = 0; j < LegendDataGrid.SelectedItems.Count; j ++)
                {
                    int i = LegendDataGrid.Items.IndexOf(LegendDataGrid.SelectedItems[j]);

                    ((KeyboardKey)LayoutListView.SelectedItem).Legends[i] = new LayerLegend(i, n.Answer.Name, n.Answer.Group);
                }
                //int i = LegendDataGrid.SelectedIndex;

                //((KeyboardKey)LayoutListView.SelectedItem).Legends[i] = new LayerLegend(i, n.Answer.Name, n.Answer.Group);
                NeedSave();
                LegendDataGrid.Items.Refresh();
                LayoutListView.Items.Refresh();
            }

        }

        private IEnumerable<DataGridRow> GetDataGridRowsForButtons(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row & row.IsSelected) yield return row;
            }
        }

        private void LayerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LayoutListView.Items.Count > 0 && ((ListBox)sender).SelectedIndex > -1)
            {
                foreach (KeyboardKey k in LayoutListView.Items)
                {
                    k.ActiveLayerId = ((KeyboardLayer)((ListBox)sender).SelectedItem).Priority;
                }

                LayoutListView.Items.Refresh();

                int i = LayoutListView.SelectedIndex;

                LayoutListView.SelectedIndex = -1;
                LayoutListView.SelectedIndex = i;

            }

        }

        private void IndicatorShapeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IndicatorTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IndicatorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IndicatorListBox.SelectedIndex !=-1)
            {

                if (((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Count > 0)
                {
                    StatusListBox.SelectedIndex = 0;
                    StatusListBox.Items.Refresh();
                }

            }

            /*
            switch ((IndicatorType)IndicatorTypeComboBox.SelectedItem)
            {
                case IndicatorType.Layer:
                    ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Clear();
                    StatusListBox.Items.Refresh();
                    StatusListBox.IsEnabled = false;
                    AddStatusButton.IsEnabled = false;
                    DeleteStatusButton.IsEnabled = false;
                    StatusNameTextBox.IsEnabled = false;
                    StatusColorButton.IsEnabled = false;
                    break;
                case IndicatorType.CAPS:
                    ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Clear();
                    ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Add(new IndicatorStatus(0, "Off"));
                    ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Add(new IndicatorStatus(1, "On"));
                    StatusListBox.Items.Refresh();
                    StatusListBox.IsEnabled = true;
                    AddStatusButton.IsEnabled = false;
                    DeleteStatusButton.IsEnabled = false;
                    StatusNameTextBox.IsEnabled = false;
                    StatusColorButton.IsEnabled = true;
                    break;
                default:
                    ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Clear();
                    StatusListBox.Items.Refresh();
                    StatusListBox.IsEnabled = true;
                    AddStatusButton.IsEnabled = true;
                    DeleteStatusButton.IsEnabled = true;
                    StatusNameTextBox.IsEnabled = true;
                    StatusColorButton.IsEnabled = true;
                    break;
            }
            */
        }

        private void IndicatorNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void StatusColorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CopyKeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            string n = GetName();

            if (n != "")
            {
                Keyboard k = new Keyboard(n, (Keyboard)KeyboardComboBox.SelectedItem);


                keyboards.Add(k);
                NeedSave();

                SortKeyboard();
                KeyboardComboBox.Items.Refresh();
                KeyboardComboBox.SelectedItem = k;
            }
        }


        private void NeedSave()
        {
            needsave = true;
        }

        private string GetName()
        {


            string s = Microsoft.VisualBasic.Interaction.InputBox("Enter a new name", "Enter Name");

            if (s == "")
            {
                return "";
            }

            if (keyboards.FindIndex(x => x.Name == s) > -1)
            {
                MessageBox.Show("A Keyboard with that name already exists.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return "";
            }

            return s;
        }

        private void AddKeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            string n = GetName();

            if (n != "")
            {
                Keyboard k = new Keyboard(n);

                keyboards.Add(k);
                NeedSave();

                SortKeyboard();
                KeyboardComboBox.Items.Refresh();
                KeyboardComboBox.SelectedItem = k;
            }
        }

        public void SortKeyboard()
        {
            keyboards.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        private void DeleteKeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this keyboard?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int i = KeyboardComboBox.SelectedIndex;
                keyboards.Remove((Keyboard)KeyboardComboBox.SelectedItem);
                KeyboardComboBox.Items.Refresh();
                NeedSave();

                KeyboardComboBox.SelectedIndex = i - 1;

                if (KeyboardComboBox.Items.Count == 0)
                {
                    KeyboardComboBox.SelectedItem = null;
                    MainTabControl.SelectedIndex = 0;
                }
            }

        }

        private void KeyboardNameButton_Click(object sender, RoutedEventArgs e)
        {
            string n = GetName();


            if (n != "")
            {
                Keyboard k = ((Keyboard)KeyboardComboBox.SelectedItem);
                k.Name = n;
                SortKeyboard();
                KeyboardComboBox.Items.Refresh();
                KeyboardComboBox.SelectedItem = k;
                NeedSave();
            }
        }

        private void CopyLayerButton_Click(object sender, RoutedEventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Enter destination layer", "Enter Layer");

            int i = LayerListBox.SelectedIndex;

            int j = -1;

            try
            {
                j = string.IsNullOrEmpty(s) ? -1 : int.Parse(s);
            }
            catch { }

            if (j > -1 && j < QMK_Assistant.Properties.Settings.Default.LayerMax)
            {
                if (MessageBox.Show("Are you sure you want to overwrate any existing legends?", "Confirm Copy", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    foreach (KeyboardKey k in ((Keyboard)KeyboardComboBox.SelectedItem).Keys)
                    {
                        LayerLegend l = new LayerLegend(j, k.Legends[i].Name, k.Legends[i].Group); 
                        k.Legends[j] = l;


                    }
                    NeedSave();
                    LayoutListView.Items.Refresh();
                    LayerListBox.SelectedIndex = j;
                }

            }
            else
            {
                MessageBox.Show("Enter a valid layer number.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearLayerButton_Click(object sender, RoutedEventArgs e)
        {
            int i = LayerListBox.SelectedIndex;
            if (MessageBox.Show("Are you sure you want to remove existing legends?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (KeyboardKey k in ((Keyboard)KeyboardComboBox.SelectedItem).Keys)
                {
                    k.Legends[i].Name = "(Blank)";
                    k.Legends[i].Group = "(Blank)";

                }
                NeedSave();
                LayoutListView.Items.Refresh();
            }
        }

        private void AddKeyButton_Click(object sender, RoutedEventArgs e)
        {
            NeedSave();
            keyboards[KeyboardComboBox.SelectedIndex].Keys.Add(new KeyboardKey(((KeyboardLayer)LayerListBox.SelectedItem).Priority));
            LayoutListView.Items.Refresh();
        }

        private void DeleteKeyButton_Click(object sender, RoutedEventArgs e)
        {
            int i = LayoutListView.SelectedIndex;
            if (i > -1)
            {
                if (MessageBox.Show("Are you sure you want to delete key legends?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NeedSave();
                    keyboards[KeyboardComboBox.SelectedIndex].Keys.RemoveAt(i);
                    LayoutListView.Items.Refresh();
                }

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (needsave)
            {
                MessageBoxResult x = MessageBox.Show("Do you want to save", "Save", MessageBoxButton.YesNoCancel);
                if (x == MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (x == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void KeyUD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //NeedSave();
            if (LayoutListView != null)
            {
                if (LayoutListView.Items.Count > 0)
                {
                    LayoutListView.Items.Refresh();
                }
            }

        }

        public event EventHandler<SaveKeyboardsEventArgs> Saving;


        public void OnSave()
        {
            if (Saving != null)
            {
                Saving(this, new SaveKeyboardsEventArgs(keyboards));
            }
        }

        private void Save()
        {
            OnSave();
            needsave = false;

            //Assistant.Keyboards.Clear();
            //foreach (Keyboard k in keyboards)
            //{
            //    Assistant.Keyboards.Add((Keyboard)k.Clone());
            //}

            //Assistant.SaveKeyboard();
            //needsave = false;
            //MessageBox.Show("Save complete. Changes will not be reflected until the application restarts.", "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //NeedSave();
        }



        private void AddIndicatorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddCustomIndicatorButton_Click(object sender, RoutedEventArgs e)
        {

            string s = Microsoft.VisualBasic.Interaction.InputBox("Enter a code", "Enter Code");
            int ix = 0;
            bool b = int.TryParse(s, out ix);
            if (!b || ix <0)
            {
                MessageBox.Show("Please enter a positive integer.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (keyboards[KeyboardComboBox.SelectedIndex].Indicators.FindIndex(x => x.Code == ix) > -1)
            {
                MessageBox.Show("An indicator with that code already exists.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            KeyboardIndicator i = new KeyboardIndicator(ix, IndicatorType.Custom);
            i.Statuses.Add(new IndicatorStatus(0, "Base Statuas"));
            keyboards[KeyboardComboBox.SelectedIndex].Indicators.Add(i);
            NeedSave();
            IndicatorListBox.Items.Refresh();
        }

        private void AddCAPSIndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (keyboards[KeyboardComboBox.SelectedIndex].Indicators.FindIndex(x => x.Type == IndicatorType.CAPS) > -1)
            {
                MessageBox.Show("A CAPS indicator already exists for this keyboard", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            KeyboardIndicator i = new KeyboardIndicator(-1, IndicatorType.CAPS);
            i.Name = "CAPS";
            i.Statuses.Add(new IndicatorStatus(0, "Off"));
            i.Statuses.Add(new IndicatorStatus(1, "On"));

            keyboards[KeyboardComboBox.SelectedIndex].Indicators.Add(i);
            NeedSave();
            IndicatorListBox.Items.Refresh();

        }

        private void DeleteIndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            keyboards[KeyboardComboBox.SelectedIndex].Indicators.RemoveAt(IndicatorListBox.SelectedIndex);
            IndicatorListBox.Items.Refresh();
        }

        private void AddLayerIndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (keyboards[KeyboardComboBox.SelectedIndex].Indicators.FindIndex(x => x.Type == IndicatorType.Layer) > -1)
            {
                MessageBox.Show("A Layer indicator already exists for this keyboard", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            KeyboardIndicator i = new KeyboardIndicator(-2, IndicatorType.Layer);
            i.Name = "Layer";

            keyboards[KeyboardComboBox.SelectedIndex].Indicators.Add(i);
            NeedSave();
            IndicatorListBox.Items.Refresh();
        }

        private void DeleteStatusButton_Click(object sender, RoutedEventArgs e)
        {

            if (((KeyboardIndicator)IndicatorListBox.SelectedItem).Type != IndicatorType.Custom)
            {
                MessageBox.Show("The indicator type" + ((KeyboardIndicator)IndicatorListBox.SelectedItem).Type.ToString() + " is not able to add or delete statuses.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (StatusListBox.Items.Count == 1)
            {
                MessageBox.Show("Indicator must have at least one status.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (StatusListBox.SelectedIndex > -1)
            {
                ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.RemoveAt(StatusListBox.SelectedIndex);
                StatusListBox.Items.Refresh();
            }

        }

        private void AddStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (IndicatorListBox.SelectedIndex > -1)
            {
                if (((KeyboardIndicator)IndicatorListBox.SelectedItem).Type != IndicatorType.Custom)
                {
                    MessageBox.Show("The indicator type" + ((KeyboardIndicator)IndicatorListBox.SelectedItem).Type.ToString() + " is not able to add or delete statuses.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

            ((KeyboardIndicator)IndicatorListBox.SelectedItem).Statuses.Add(new IndicatorStatus(GetNextStatusCode(), "Code " + GetNextStatusCode().ToString()));
                StatusListBox.Items.Refresh();
            }


        }

        private int GetNextStatusCode()
        {
            int i = 0;

            foreach (IndicatorStatus s in StatusListBox.Items)
            {
                i = Math.Max(i, s.Code);
            }
            return i + 1;


        }

        private void UD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //NeedSave();
        }

        private void ColorSelector_ValueChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            //NeedSave();
        }

        string interactionstart = "";
        string keycodestart = "";


        private void InteractionTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            foreach (TextBox b in InteractionGrid.Children.OfType<TextBox>())
            {
                if (t.Text == b.Text && t != b)
                {
                    MessageBox.Show("This Id already exists in QMK Interaction keyus.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    t.Text = interactionstart;

                    return;

                }

            }

            if (t.Text != interactionstart)
            {
                NeedSave();
            }
        }


        private void InteractionTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            interactionstart = t.Text;
        }



        private void KeycodeTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            foreach (TextBox b in InteractionGrid.Children.OfType<TextBox>())
            {
                if (t.Text == b.Text && t != b)
                {
                    MessageBox.Show("This Id already exists in QMK Interaction keyus.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    t.Text = keycodestart;

                    return;

                }

                if (t.Text != keycodestart)
                {
                    NeedSave();
                }
            }
        }

        private void KeycodeTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            keycodestart = t.Text;
        }

        private int indicatorstart;
        private void IndicatorCodeTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            int i;
            bool b = int.TryParse(((TextBox)sender).Text, out i);
            if(b)
            indicatorstart = i;
        }

        private void IndicatorCodeTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            int ix = 0;
            bool b = int.TryParse(t.Text, out ix);
            if (!b || ix <0)
            {
                MessageBox.Show("Please enter a positive integer.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                t.Text = indicatorstart.ToString();
                return;
            }

            if (keyboards[KeyboardComboBox.SelectedIndex].Indicators.FindIndex(x => x.Code == ix) > -1)
            {
                MessageBox.Show("An indicator with that code already exists.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                t.Text = indicatorstart.ToString();
                return;
            }

            if (ix != indicatorstart)
            {
                NeedSave();
            }
        }

        string layernamestart;
        private void LayerNameTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            layernamestart = ((TextBox)sender).Text;
        }

        private void LayerNameTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;

            for(int i = 0; i < keyboards[KeyboardComboBox.SelectedIndex].Layers.Length; i++)
            {
                if (keyboards[KeyboardComboBox.SelectedIndex].Layers[i].Priority != ((KeyboardLayer)LayerListBox.SelectedItem).Priority && t.Text == keyboards[KeyboardComboBox.SelectedIndex].Layers[i].Name)
                {
                    MessageBox.Show("An indicator with that code already exists.", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    t.Text = layernamestart.ToString();
                    return;
                }
            }

            if (t.Text != layernamestart)
            {
                NeedSave();
            }

        }

        string colorstart;
        private void ColorSelector_GotFocus(object sender, RoutedEventArgs e)
        {
            colorstart = ((ColorSelector)sender).ColorHex;
        }

        private void ColorSelector_LostFocus(object sender, RoutedEventArgs e)
        {
            if(colorstart != ((ColorSelector)sender).ColorHex)
            {
                NeedSave();
            }
        }

        int indicatorshapestart;
        private void IndicatorShapeComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            indicatorshapestart = ((ComboBox)sender).SelectedIndex;
        }

        private void IndicatorShapeComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(((ComboBox)sender).SelectedIndex != indicatorshapestart)
            {
                NeedSave();
            }
        }

        private double updownstart;

        private void UpDown_GotFocus(object sender, RoutedEventArgs e)
        {
            updownstart = ((NumericUpDown)sender).Value;
        }

        private void UpDown_LostFocus(object sender, RoutedEventArgs e)
        {
            if(((NumericUpDown)sender).Value != updownstart)
            {
                NeedSave();
            }
        }
    }

}

