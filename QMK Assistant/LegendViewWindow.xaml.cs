using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace QMK_Assistant
{
    /// <summary>
    /// Interaction logic for LegendViewWindow.xaml
    /// </summary>
    public partial class LegendViewWindow : Window
    {
        public LegendViewWindow(LayerLegend legend)
        {
            Legends = Assistant.KeyLegends;
            Groups = Assistant.LegendGroups;
            InitializeComponent();
            //this.DataContext = this;
            //LegendListView.Items.Refresh();

            LegendListView.ItemsSource = Assistant.KeyLegends;

            foreach(KeyLegend k in LegendListView.Items)
            {
                if (k.Name == legend.Name && k.Group == legend.Group)
                {
                    LegendListView.SelectedIndex = LegendListView.Items.IndexOf(k);
                    Answer = k;
                }
            }
            
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(LegendListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");

            if(view.GroupDescriptions.Count == 0)
            {
                view.GroupDescriptions.Add(groupDescription);
            }
            

        }

        public LegendViewWindow(List<KeyLegend> legends)
        {
            InitializeComponent();
            Legends = legends;


            //LegendListView.ItemsSource = App.KeyLegends;
        }

        private KeyLegend answer = null;

        public static List<KeyLegend> Legends = new List<KeyLegend>();
        public static List<string> Groups = new List<string>();

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public KeyLegend Answer
        {
            get
            {
                return answer;
            }
            set
            { 
                answer = value;
                if(answer != null)
                {
                    SelectionTextBlock.Text = answer.Name;
                }
                else
                {
                    SelectionTextBlock.Text = "";
                }
                
            }
        }

        private void LegendListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Answer = (KeyLegend)LegendListView.SelectedItem;
            LegendListView.Items.Refresh();
        }
    }
}
