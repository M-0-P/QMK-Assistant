using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using HidLibrary;
namespace QMK_Assistant
{
    public class Assistant
    {
        #region InitialSetUp


        public List<OverlaySize> OverlaySizes = new List<OverlaySize>();
        public List<OverlayPosition> OverlayPositions = new List<OverlayPosition>();



       public void Start()
        {


            ImportIndicatorShapes();
            ImportKeyLegends();
            ImportKeyboards();

            //CreateQMKKeyDictionary();

            PopulateOverlayLists();
            CreateContextMenu();
            InitializeWatchers();
            UpdateHIDevices(false);
            StartWatchingingForDeviceEvents();

            if (activeKeyboard != null)
            {
                InitialDisplay();
            }


        }

        private void PopulateOverlayLists()
        {
            string str = QMK_Assistant.Properties.Settings.Default.OverlaySizes;

            string[] sizeraw = str.Split(';');

            foreach (string s in sizeraw)
            {
                string[] data = s.Split(',');

                if (data.Length == 2)
                {
                    OverlaySize v = new OverlaySize(double.Parse(data[0]), data[1]);
                    OverlaySizes.Add(v);
                }
            }


            string strP = QMK_Assistant.Properties.Settings.Default.OverlayPositions;

            string[] posraw = strP.Split(';');

            foreach (string s in posraw)
            {
                string[] data = s.Split(',');

                if (data.Length == 5)
                {
                    OverlayPosition v = new OverlayPosition(
                                                                data[0],
                                                                (OverlayHorizontalAlignment)Enum.Parse(typeof(OverlayHorizontalAlignment), data[1]),
                                                                double.Parse(data[2]),
                                                                (OverlayVerticalAlignment)Enum.Parse(typeof(OverlayVerticalAlignment),
                                                                data[3]),
                                                                double.Parse(data[4])
                                                           );
                    OverlayPositions.Add(v);
                }
            }

        }


        private void SaveOverlaySettings()
        {
            foreach (MenuItem i in miOverlayDisplay.MenuItems)
            {
                if (i.Checked)
                {
                    QMK_Assistant.Properties.Settings.Default.DefaultOverlayDisplay = ((OverlayType)i.Tag).ToString();
                }
            }

            foreach (MenuItem i in miOverlayPosition.MenuItems)
            {
                if (i.Checked)
                {
                    QMK_Assistant.Properties.Settings.Default.DefaultOverlayPosition = ((OverlayPosition)i.Tag).Name;
                }
            }

            foreach (MenuItem i in miOverlaySize.MenuItems)
            {
                if (i.Checked)
                {
                    QMK_Assistant.Properties.Settings.Default.DefaultOverlayPosition = ((OverlaySize)i.Tag).Name;
                }
            }

            foreach (MenuItem i in miOverlayOpacity.MenuItems)
            {
                if (i.Checked)
                {
                    QMK_Assistant.Properties.Settings.Default.DefaultOverlayPosition = ((double)i.Tag).ToString();
                }
            }
            QMK_Assistant.Properties.Settings.Default.DefaultTrackKeystroke = miTrackKeys.Checked;

            QMK_Assistant.Properties.Settings.Default.Save();
        }

        private void InitialDisplay()
        {
            OverlayType t = (OverlayType)Enum.Parse(typeof(OverlayType), QMK_Assistant.Properties.Settings.Default.DefaultOverlayDisplay);
            OverlayPosition p = OverlayPositions.Find(x => x.Name == QMK_Assistant.Properties.Settings.Default.DefaultOverlayPosition);
            OverlaySize s = OverlaySizes.Find(x => x.Name == QMK_Assistant.Properties.Settings.Default.DefaultOverlaySize);
            double o = QMK_Assistant.Properties.Settings.Default.DefaultOverlayOpacity;


            foreach (MenuItem i in miOverlayDisplay.MenuItems)
            {
                if ((OverlayType)i.Tag == t)
                {
                    MiOverlayDisplay_Click(i, new EventArgs());
                }
            }

            foreach (MenuItem i in miOverlayPosition.MenuItems)
            {
                if ((OverlayPosition)i.Tag == p)
                {
                    MiOverlayPosition_Click(i, new EventArgs());
                }
            }

            foreach (MenuItem i in miOverlaySize.MenuItems)
            {
                if ((OverlaySize)i.Tag == s)
                {
                    MiOverlaySize_Click(i, new EventArgs());
                }
            }

            foreach (MenuItem i in miOverlayOpacity.MenuItems)
            {
                if ((double)i.Tag == o)
                {
                    MiOverlayOpacity_Click(i, new EventArgs());
                }
            }

            miTrackKeys.Checked = QMK_Assistant.Properties.Settings.Default.DefaultTrackKeystroke;
            //overlay.UpdateLayer(0);
        }


        #endregion


        #region MenuItems
        MenuItem miExit = new MenuItem();
        MenuItem miDisplaySave = new MenuItem();
        MenuItem miTrackKeys = new MenuItem();
        MenuItem miMonitor = new MenuItem();
        MenuItem miDevices = new MenuItem();
        MenuItem miKeyboard = new MenuItem();
        MenuItem miMacros = new MenuItem();
        MenuItem miLegends = new MenuItem();
        MenuItem miEditKeyboards = new MenuItem();

        MenuItem miKeyboardOverlays = new MenuItem();

        MenuItem miOverlayOptions = new MenuItem();
        MenuItem miOverlayOpacity = new MenuItem();

        MenuItem miOverlayDisplay = new MenuItem();

        MenuItem miOverlaySize = new MenuItem();
        MenuItem miOverlayPosition = new MenuItem();

        private void CreateContextMenu()
        {
            miExit.Index = 0;
            miExit.Text = "Exit";
            miExit.Click += MiExit_Click;
            miDevices.Index = 1;
            miDevices.Text = "Monitor Devices";
            miDevices.Checked = QMK_Assistant.Properties.Settings.Default.MonitorDefault;
            miDevices.Click += MiDevices_Click;




            miEditKeyboards.Index = 1;
            miEditKeyboards.Text = "Edit Keyboards";
            miEditKeyboards.Click += MiEditKeyboards_Click;


            miOverlayOptions.Index = 2;
            miOverlayOptions.Text = "Overlay Options";

            miOverlayDisplay.Index = 3;
            miOverlayDisplay.Text = "Display";

            miOverlaySize.Index = 4;
            miOverlaySize.Text = "Size";

            miOverlayPosition.Index = 5;
            miOverlayPosition.Text = "Position";

            miOverlayOpacity.Index = 6;
            miOverlayOpacity.Text = "Opacity";

            miTrackKeys.Index = 7;
            miTrackKeys.Text = "Track Keystrokes";
            miTrackKeys.Click += MiTrackKeys_Click;

            miDisplaySave.Index = 8;
            miDisplaySave.Text = "Save Current Overlay Settings";
            miDisplaySave.Click += MiDisplaySave_Click;



            miKeyboardOverlays.Index = 9;
            miKeyboardOverlays.Text = "Available Keyboard Overlays";





            foreach (OverlayType v in Enum.GetValues(typeof(OverlayType)))
            {
                MenuItem MiOverlayDisplay = new MenuItem();

                MiOverlayDisplay.Tag = v;
                MiOverlayDisplay.Text = v.ToString();
                MiOverlayDisplay.Click += MiOverlayDisplay_Click;

                miOverlayDisplay.MenuItems.Add(MiOverlayDisplay);
            }


            foreach (OverlaySize v in OverlaySizes)
            {
                MenuItem MiOverlaySize = new MenuItem();

                MiOverlaySize.Tag = v;
                MiOverlaySize.Text = v.Name;
                MiOverlaySize.Click += MiOverlaySize_Click;

                miOverlaySize.MenuItems.Add(MiOverlaySize);
            }

            foreach (OverlayPosition v in OverlayPositions)
            {
                MenuItem MiOverlayPosition = new MenuItem();

                MiOverlayPosition.Tag = v;
                MiOverlayPosition.Text = v.Name;
                MiOverlayPosition.Click += MiOverlayPosition_Click;

                miOverlayPosition.MenuItems.Add(MiOverlayPosition);
            }


            for (double j = 0 + QMK_Assistant.Properties.Settings.Default.OverlayOpacityStep; j <= 1; j += QMK_Assistant.Properties.Settings.Default.OverlayOpacityStep)
            {
                MenuItem MiOverlayOpacity = new MenuItem();


                MiOverlayOpacity.Tag = j;
                MiOverlayOpacity.Text = (j * 100).ToString() + "%";
                MiOverlayOpacity.Click += MiOverlayOpacity_Click;

                miOverlayOpacity.MenuItems.Add(MiOverlayOpacity);

            }

            miOverlayOptions.MenuItems.Add(miDisplaySave);
            miOverlayOptions.MenuItems.Add("-");
            miOverlayOptions.MenuItems.Add(miTrackKeys);
            miOverlayOptions.MenuItems.Add(miKeyboardOverlays);
            miOverlayOptions.MenuItems.Add(miOverlayDisplay);
            miOverlayOptions.MenuItems.Add(miOverlayPosition);
            miOverlayOptions.MenuItems.Add(miOverlaySize);
            miOverlayOptions.MenuItems.Add(miOverlayOpacity);

            App.notifyIcon.ContextMenu = new ContextMenu();

            App.notifyIcon.ContextMenu.MenuItems.Add(miExit);
            App.notifyIcon.ContextMenu.MenuItems.Add("-");
            App.notifyIcon.ContextMenu.MenuItems.Add(miDevices);
            App.notifyIcon.ContextMenu.MenuItems.Add(miEditKeyboards);
            App.notifyIcon.ContextMenu.MenuItems.Add(miOverlayOptions);

        }

        private void MiTrackKeys_Click(object sender, EventArgs e)
        {
            ((MenuItem)sender).Checked = !((MenuItem)sender).Checked;
        }

        private void MiDisplaySave_Click(object sender, EventArgs e)
        {
            SaveOverlaySettings();
        }

        private void MiDevices_Click(object sender, EventArgs e)
        {
            miDevices.Checked = !miDevices.Checked;
            if (miDevices.Checked)
            {
                UpdateHIDevices(false);
                StartWatchingingForDeviceEvents();
            }
            else
            {
                StopWatchingingForDeviceEvents();
            }

        }

        private int DevicesIndex = 0;
        private int OverlayOpacityIndex = 0;
        private int OverlaySizeIndex = 0;
        private int OverlayTypeIndex = 0;
        private int OverlayPositionIndex = 0;

        private void MiOverlayOpacity_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < miOverlayOpacity.MenuItems.Count; i++)
            {
                MenuItem mi = miOverlayOpacity.MenuItems[i];
                if ((MenuItem)sender == mi)
                {
                    mi.Checked = true;
                    OverlayOpacityIndex = i;
                }
                else
                {
                    mi.Checked = false;
                }
            }

            overlay.UpdateOverlayOpacity((double)((MenuItem)sender).Tag);
        }

        private void MiOverlayPosition_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < miOverlayPosition.MenuItems.Count; i++)
            {
                MenuItem mi = miOverlayPosition.MenuItems[i];
                if ((MenuItem)sender == mi)
                {
                    mi.Checked = true;
                    OverlayPositionIndex = i;
                }
                else
                {
                    mi.Checked = false;
                }
            }

            overlay.UpdateOverlayPosition((OverlayPosition)((MenuItem)sender).Tag);
        }

        private void MiOverlaySize_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < miOverlaySize.MenuItems.Count; i++)
            {
                MenuItem mi = miOverlaySize.MenuItems[i];
                if ((MenuItem)sender == mi)
                {
                    mi.Checked = true;
                    OverlaySizeIndex = i;
                }
                else
                {
                    mi.Checked = false;
                }
            }

            overlay.UpdateOverlaySize((OverlaySize)((MenuItem)sender).Tag);
        }

        private void MiOverlayDisplay_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < miOverlayDisplay.MenuItems.Count; i++)
            {
                MenuItem mi = miOverlayDisplay.MenuItems[i];
                if ((MenuItem)sender == mi)
                {
                    mi.Checked = true;
                    OverlayTypeIndex = i;
                }
                else
                {
                    mi.Checked = false;
                }
            }
            overlay.UpdateOverlayType((OverlayType)((MenuItem)sender).Tag);
        }

        private void MiEditKeyboards_Click(object sender, EventArgs e)
        {

            EditKeyboardWindow w = new EditKeyboardWindow(Keyboards);
            w.Saving += EditKeyboardWindow_Saving;
            w.ShowDialog();
            w = null;
        }

        private void EditKeyboardWindow_Saving(object sender, SaveKeyboardsEventArgs e)
        {
            Keyboards = e.Keyboards;
            //Keyboards.Clear();
            //foreach (Keyboard k in e.Keyboards)
            //{
            //    Keyboards.Add((Keyboard)k.Clone());
            //}
            SaveKeyboard();
        }

        private void MiExit_Click(object sender, EventArgs e)
        {
            App.notifyIcon.Dispose();
            Environment.Exit(0);
        }


        #endregion



        #region Monitoring

        ManagementEventWatcher CreationWatcher = new ManagementEventWatcher();
        ManagementEventWatcher DeletionWatcher = new ManagementEventWatcher();

        //HidDevice ActiveDevice;
        HidFastReadDevice ActiveDevice;
        HidFastReadEnumerator fastReadEnumerator = new HidFastReadEnumerator();

        private IEnumerable<HidDevice> GetListableDevices() =>
                HidDevices.Enumerate()
                .Where(d => d.IsConnected)
                .Where(device => device.Capabilities.InputReportByteLength > 0)
                .Where(device => (ushort)device.Capabilities.UsagePage == 0xFF31)
                .Where(device => (ushort)device.Capabilities.Usage == 0x0074);

        private void InitializeWatchers()
        {
            CreationWatcher.Query.QueryString = $"SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            CreationWatcher.EventArrived += DeviceEvent;
            DeletionWatcher.Query.QueryString = $"SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'";
            DeletionWatcher.EventArrived += DeviceEvent;
        }

        private void StartWatchingingForDeviceEvents()
        {
            CreationWatcher.Start();
            DeletionWatcher.Start();
        }

        private void StopWatchingingForDeviceEvents()
        {
            CreationWatcher.Stop();
            DeletionWatcher.Stop();
        }

        private void WatchForDeviceChanges(string eventType)
        {
            var watcher = new ManagementEventWatcher($"SELECT * FROM {eventType} WITHIN 2 WHERE TargetInstance ISA 'Win32_PnPEntity'");
            watcher.EventArrived += DeviceEvent;
            watcher.Start();
        }

        private void DeviceEvent(object sender, EventArrivedEventArgs e)
        {
            (sender as ManagementEventWatcher)?.Stop();

            if (!(e.NewEvent["TargetInstance"] is ManagementBaseObject instance))
            {
                return;
            }

            var deviceDisconnected = e.NewEvent.ClassPath.ClassName.Equals("__InstanceDeletionEvent");

            UpdateHIDevices(deviceDisconnected);

            (sender as ManagementEventWatcher)?.Start();
        }

        private void GetInitialKeyboardInfo()
        {

        }


        private List<HidDevice> AvailableDevices = new List<HidDevice>();

        private void UpdateHIDevices(bool disconnected)
        {
            List<HidDevice> _AvailableDevices = GetListableDevices().ToList();

            if (miDevices.Checked)
            {


                if (!disconnected)
                {

                    var AddDevices = _AvailableDevices.Where(x => !AvailableDevices.Any(y => x.DevicePath == y.DevicePath));


                    foreach (HidDevice device in AddDevices)
                    {
                        MenuItem miDevice = new MenuItem();
                        miDevice.Tag = device;
                        miDevice.Text = GetDeviceInfo(device);
                        miDevice.Click += MiDevice_Click;
                        miDevices.MenuItems.Add(miDevice);
                    }
                }
                else
                {
                    var RemoveDevices = AvailableDevices.Where(x => !_AvailableDevices.Any(y => x.DevicePath == y.DevicePath));

                    foreach (HidDevice device in RemoveDevices)
                    {
                        for (int i = miDevices.MenuItems.Count - 1; i >= 0; i--)
                        {
                            if (((HidDevice)miDevices.MenuItems[i].Tag).DevicePath == device.DevicePath)
                            {
                                miDevices.MenuItems.RemoveAt(i);
                            }
                        }
                    }
                }

                AvailableDevices = _AvailableDevices;


                bool Checked = false;
                foreach (MenuItem mi in miDevices.MenuItems)
                {
                    if (mi.Checked)
                    {
                        Checked = mi.Checked;
                    }
                }

                if (miDevices.MenuItems.Count == 0)
                {
                    GetKeyboardOverlays(null); //mop fix this
                }

                if (miDevices.Checked && !Checked && miDevices.MenuItems.Count > 0)
                {
                    miDevices.MenuItems[0].PerformClick();
                }

            }


        }

        int DeviceIndex = 0;
        int KeyboardIndex = 0;

        private void MiDevice_Click(object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            if (!mi.Checked)
            {

                for (int j = 0; j < miDevices.MenuItems.Count; j++)
                {
                    MenuItem i = miDevices.MenuItems[j];
                    if (i == mi)
                    {
                        i.Checked = true;
                        DeviceIndex = j;
                        //ActiveDevice = (HidDevice)i.Tag;
                        ActiveDevice = (HidFastReadDevice)fastReadEnumerator.GetDevice(((HidDevice)i.Tag).DevicePath);

                        ActiveDevice.OpenDevice();
                        //SetActiveKeyboard(GetDeviceProperties(ActiveDevice));
                        GetKeyboardOverlays(GetDeviceProperties(ActiveDevice));

                        ActiveDevice.MonitorDeviceEvents = true;
                        ActiveDevice.FastReadReport(OnReport);


                        ActiveDevice.ReadReport(OnReport);
                        GetInitialKeyboardInfo();
                        ActiveDevice.CloseDevice();

                        byte[] data = new byte[63];
                        //data[0] = (byte)0x00;
                        //data[1] = (byte)12; // mao ni ang 0 sa PIC
                        ////data[2] = deviceId; // 1
                        ////data[3] = deviceState; // 2
                        //HidReport report = new HidReport(31, new HidDeviceData(data, HidDeviceData.ReadStatus.Success)); //7
                        //ActiveDevice.WriteReport(report);


                    }
                    else
                    {
                        i.Checked = false;
                    }
                }

            }
            else
            {
                mi.Checked = false;
            }

        }

        private void OnReport(HidReport report)
        {

            if (miDevices.Checked)
            {
                var data = report.Data;

                var outputString = string.Empty;
                for (var i = 0; i < data.Length; i++)
                {
                    outputString += (char)data[i];

                }
                if(activeKeyboard != null)
                {
                    string[] output = outputString.Split('\n');

                    try
                    {
                        foreach (string s in output)
                        {

                            if (s.StartsWith(prefix) && s.EndsWith(suffix))
                            {
                                ReadQMKOutput(s.Substring(prefix.Length, s.Length - (prefix.Length + suffix.Length)));
                            }
                        }
                    }
                    catch { }


                }


                ActiveDevice.ReadReport(OnReport);


            }

        }

        public string GetDeviceInfo(IHidDevice d)
        {

            return GetManufacturerString(d) + ":" + GetProductString(d) + ":" + Convert.ToString(d.Attributes.VendorId, 16) + ":" + Convert.ToString(d.Attributes.ProductId, 16) + ":" + Convert.ToString(d.Attributes.Version, 16);

        }

        public string GetDeviceProperties(IHidDevice d)
        {

            return Convert.ToString(d.Attributes.VendorId, 16) + ":" + Convert.ToString(d.Attributes.ProductId, 16) + ":" + Convert.ToString(d.Attributes.Version, 16);

        }

        private string GetProductString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadProduct(out var bs);
            return System.Text.Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        private string GetManufacturerString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadManufacturer(out var bs);
            return System.Text.Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        #endregion


        #region KeyLegends
        public static List<KeyLegend> KeyLegends { get; set; } = new List<KeyLegend>();
        public static List<string> LegendGroups { get; set; } = new List<string>();


        public void ImportKeyLegends()
        {

            KeyLegend legend = new KeyLegend();

            legend.Name = "(Blank)";
            legend.HeightU = 1;
            legend.WidthU = 1;
            legend.Group = "(Blank)";
            legend.PathData = "";

            KeyLegends.Add(legend);
            string grp;
            FileInfo[] files;
            XmlDocument x = new XmlDocument();
            XmlNodeList nodes;
            double w, h;

            DirectoryInfo d = new DirectoryInfo(QMK_Assistant.Properties.Settings.Default.LegendsPath);

            DirectoryInfo[] groups = d.GetDirectories();

            foreach (DirectoryInfo g in groups)
            {
                grp = g.Name;

                LegendGroups.Add(g.Name);
                files = g.GetFiles("*.xaml", SearchOption.TopDirectoryOnly);

                double b = QMK_Assistant.Properties.Settings.Default.BaseUnit;
                double l = QMK_Assistant.Properties.Settings.Default.DefaultSpacing;
                double m = QMK_Assistant.Properties.Settings.Default.LegendMargin;

                double bl = b * l;

                foreach (FileInfo i in files)
                {
                    try
                    {
                        KeyLegend k = new KeyLegend();
                        k.Name = i.Name.Replace(i.Extension, "");
                        k.Group = g.Name;

                        x.Load(i.FullName);
                        nodes = x.GetElementsByTagName("*");


                        foreach (XmlNode n in nodes)
                        {
                            switch (n.Name)
                            {
                                case "Canvas":
                                    if (n.Attributes[0].Value == "svg8")
                                    {
                                        w = double.Parse(n.Attributes[1].Value);
                                        h = double.Parse(n.Attributes[2].Value);

                                        if ((w + m) % bl == 0 || (h + m) % bl == 0)
                                        {
                                            k.WidthU = (w + m) / b;
                                            k.HeightU = (h + m) / b;
                                        }
                                    }
                                    break;
                                case "PathGeometry":
                                    k.LegendPath = n.Attributes[0].Value;
                                    break;

                                default:

                                    break;

                            }
                        }

                        KeyLegends.Add(k);

                    }
                    catch
                    {

                    }


                }



            }

        }

        public void SortKeyLegends()
        {
            KeyLegends.Sort((x, y) => x.Name.CompareTo(y.Name));
        }




        #endregion


        #region Indictors

        public static List<IndicatorShape> IndicatorShapes { get; set; } = new List<IndicatorShape>();

        public void ImportIndicatorShapes()
        {

            XmlDocument x = new XmlDocument();
            XmlNodeList nodes;

            FileInfo[] files;

            DirectoryInfo d = new DirectoryInfo(QMK_Assistant.Properties.Settings.Default.IndicatorShapePath);
            files = d.GetFiles("*.xaml", SearchOption.TopDirectoryOnly);


            foreach (FileInfo i in files)
            {
                try
                {
                    IndicatorShape k = new IndicatorShape();
                    k.Name = i.Name.Replace(i.Extension, "");

                    x.Load(i.FullName);
                    nodes = x.GetElementsByTagName("*");


                    foreach (XmlNode n in nodes)
                    {
                        switch (n.Name)
                        {
                            case "Canvas":
                                if (n.Attributes[0].Value == "svg8")
                                {
                                    //Add confirm 32 x 32 code
                                }
                                break;
                            case "PathGeometry":
                                k.ShapePath = n.Attributes[0].Value;
                                break;

                            default:

                                break;

                        }
                    }

                    IndicatorShapes.Add(k);

                }
                catch
                {

                }


            }

        }

        #endregion


        #region Keyboards
        public List<Keyboard> Keyboards { get; set; } = new List<Keyboard>();

        public void ImportKeyboards()
        {
            FileInfo[] files;

            DirectoryInfo d = new DirectoryInfo(QMK_Assistant.Properties.Settings.Default.KeyboardPath);
            files = d.GetFiles("*.csv", SearchOption.TopDirectoryOnly);


            foreach (FileInfo f in files)
            {
                Keyboard k = new Keyboard();
                string[] lines;

                lines = System.IO.File.ReadAllLines(f.FullName);
                int lay = 0;

                foreach (string line in lines)
                {
                    string[] data = line.Split(',');


                    switch (data[0])
                    {
                        case "Keyboard":
                            Keyboard b = new Keyboard(data);
                            k = b;
                            //k.Name = data[1];
                            //k.WidthU = double.Parse(data[2]);
                            //k.HeightU = double.Parse(data[3]);
                            //k.KeyColor = data[4];
                            //k.VendorId = data[5];
                            //k.ProductId = data[6];
                            //k.Version = data[7];
                            break;
                        case "Layer":
                            KeyboardLayer l = new KeyboardLayer();
                            l.Name = data[1];
                            l.Priority = int.Parse(data[2]);
                            l.ColorHex = data[3];
                            l.Description = data[4];
                            k.Layers[lay] = l;
                            lay += 1;
                            break;
                        case "Macro":
                            string[] data_m = line.Split(',', '4');
                            KeyboardMacro m = new KeyboardMacro();
                            m.Layer = int.Parse(data_m[1]);
                            m.Column = (int.Parse(data_m[2]));
                            m.Row = (int.Parse(data_m[3]));
                            m.MacroType = (MacroType)Enum.Parse(typeof(MacroType), data_m[4]);
                            m.MacroText = data_m[5];
                            k.Macros.Add(m);
                            break;
                        case "Indicator":
                            KeyboardIndicator i = new KeyboardIndicator();
                            int indicators = (data.Length - 4) / 3;
                            i.Name = data[1];
                            i.Type = (IndicatorType)Enum.Parse(typeof(IndicatorType), data[2]);
                            i.IndicatorShape = data[3];
                            for (int ind = 0; ind < indicators; ind++)
                            {
                                IndicatorStatus status = new IndicatorStatus();
                                status.Code = int.Parse(data[4 + ind * 3]);
                                status.Text = data[5 + ind * 3];
                                status.Color = data[6 + ind * 3];
                                i.Statuses.Add(status);
                            }
                            k.Indicators.Add(i);
                            break;
                        case "Key":
                            KeyboardKey y = new KeyboardKey(0);
                            y.Column = int.Parse(data[1]);
                            y.Row = int.Parse(data[2]);
                            y.WidthU = double.Parse(data[3]);
                            y.HeightU = double.Parse(data[4]);
                            y.XU = double.Parse(data[5]);
                            y.YU = double.Parse(data[6]);
                            y.Rotation = int.Parse(data[7]);
                            //int x = data.Length - 8;

                            for (int ix = 0; ix < QMK_Assistant.Properties.Settings.Default.LayerMax; ix++)
                            {
                                if (2 * ix + 1 + 8 >= data.Length || KeyLegends.FindIndex(r => r.Name == data[2 * ix + 8] && r.Group == data[2 * ix + 1 + 8]) == -1)
                                {

                                    y.Legends[ix] = new LayerLegend(ix, "(Blank)", "(Blank)");
                                }
                                else
                                {
                                    y.Legends[ix] = new LayerLegend(ix, data[2 * ix + 8], data[2 * ix + 1 + 8]);
                                }
                            }

                            k.Keys.Add(y);
                            break;
                        default:
                            break;


                    }
                }

                Keyboards.Add(k);

            }


        }

        private void DeleteKeyboardFiles()
        {
            FileInfo[] files;

            DirectoryInfo d = new DirectoryInfo(QMK_Assistant.Properties.Settings.Default.KeyboardPath);
            files = d.GetFiles("*.csv", SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {

                if (Keyboards.FindIndex(x => x.Name == file.Name.Replace(file.Extension, "")) == -1)
                {
                    file.Delete();
                }
            }
        }

        public void SaveKeyboard()
        {
            DeleteKeyboardFiles();
            foreach (Keyboard b in Keyboards)
            {
                using (StreamWriter sw = new StreamWriter(QMK_Assistant.Properties.Settings.Default.KeyboardPath + "\\" + b.Name + ".csv", false))
                {
                    sw.WriteLine(b.GetSaveLine());

                    foreach (KeyboardLayer l in b.Layers)
                    {
                        sw.WriteLine("Layer," + l.Name + "," + l.Priority.ToString() + "," + l.ColorHex + "," + l.Description);
                    }

                    foreach (KeyboardMacro m in b.Macros)
                    {
                        sw.WriteLine("Macro" + "," + m.Layer.ToString() + "," + m.Column.ToString() + "," + m.Row.ToString() + "," + m.MacroType.ToString() +","+ m.MacroText) ;
                    }

                    foreach (KeyboardIndicator i in b.Indicators)
                    {
                        string si = "Indicator," + i.Name + "," + i.Type.ToString() + "," + i.IndicatorShape;

                        foreach (IndicatorStatus st in i.Statuses)
                        {
                            si = si + "," + st.Code.ToString() + "," + st.Text + "," + st.Color;
                        }

                        sw.WriteLine(si);
                    }
                    foreach (KeyboardKey k in b.Keys)
                    {
                        string s = "";
                        for (int i = 0; i < QMK_Assistant.Properties.Settings.Default.LayerMax; i++)
                        {

                            if (i <= k.Legends.Length)
                            {
                                s = s + k.Legends[i].Name + "," + k.Legends[i].Group + ",";
                            }

                        }

                        s = s.Substring(0, s.Length - 1);

                        sw.WriteLine("Key," + k.Column.ToString() + "," + k.Row.ToString() + "," + k.WidthU.ToString() + "," + k.HeightU.ToString() + "," + k.XU + "," + k.YU + "," + k.Rotation.ToString() + "," + s);



                    }
                }
            }

            
            GetKeyboardOverlays(GetDeviceProperties(ActiveDevice));
            //SetActiveKeyboard(GetDeviceProperties(ActiveDevice));

        }
        #endregion


        #region QMKMessageProcessing
        string prefix;
        string suffix;
        Keyboard activeKeyboard =null;
        OverlayWindow overlay = new OverlayWindow();

        public void GetKeyboardOverlays(string properties)
        {
            if (properties != null)
            {
                List<Keyboard> keyboards = Keyboards.FindAll(x => x.GetDeviceProperties() == properties);

                string n = null;
                
                if (miKeyboardOverlays.MenuItems.Count > 0)
                {
                    n = miKeyboardOverlays.MenuItems[KeyboardIndex].Text;

                }

                KeyboardIndex = 0;
                miKeyboardOverlays.MenuItems.Clear();
                for(int i = 0; i < keyboards.Count;i++)
                {
                    MenuItem miKeyboardOverlay = new MenuItem(keyboards[i].Name);
                    if(keyboards[i].Name == n)
                    {
                        KeyboardIndex = i;
                    }
                    miKeyboardOverlay.Click += MiKeyboardOverlay_Click;
                    miKeyboardOverlays.MenuItems.Add(miKeyboardOverlay);
                }
                if(miKeyboardOverlays.MenuItems.Count ==0)
                {
                    SetActiveKeyboard(null);
                   
                }
                else
                {
                    miKeyboardOverlays.MenuItems[KeyboardIndex].PerformClick();
                }
                
            }
            else
            {
                SetActiveKeyboard(null);
            }
        }

        private bool CheckDisplay()
        {
            bool d = false;
            bool o = false;
            bool p = false;
            bool s = false;

            foreach (MenuItem i in miOverlayDisplay.MenuItems)
            {
                if(i.Checked)
                {
                    d = true;
                }
            }
            foreach (MenuItem i in miOverlayOpacity.MenuItems)
            {
                if (i.Checked)
                {
                    o = true;
                }
            }
            foreach (MenuItem i in miOverlayPosition.MenuItems)
            {
                if (i.Checked)
                {
                    p = true;
                }
            }
            foreach (MenuItem i in miOverlaySize.MenuItems)
            {
                if (i.Checked)
                {
                    s = true;
                }
            }
            if(d && o && p && s)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void MiKeyboardOverlay_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < miKeyboardOverlays.MenuItems.Count; i++)
            {
                MenuItem mi = miKeyboardOverlays.MenuItems[i];

                if (((MenuItem)sender) == mi)
                {
                    mi.Checked = true;
                    SetActiveKeyboard(mi.Text);
                    KeyboardIndex = i;
                    if(!CheckDisplay())
                    {
                        InitialDisplay();
                    }
                   
                }
                else
                {
                    mi.Checked = false;
                }
            }
        }


        public void SetActiveKeyboard(string name)
        {
            if (name != null)
            {
                activeKeyboard = Keyboards.Find(x => x.Name == name);
            }
            else
            {
                activeKeyboard = null;
                App.Current.Dispatcher.Invoke(() =>
                {
                    overlay.ActivateKeyboard(activeKeyboard);
                });
            }


            App.Current.Dispatcher.Invoke(() =>
            {
                if (activeKeyboard != null)
                {
                    prefix = activeKeyboard.QMKStringPrefix;
                    suffix = activeKeyboard.QMKStringSuffix;
                    CreateQMKKeyDictionary();
                    overlay.ActivateKeyboard(activeKeyboard);
                }

            });

        }

        /*
        public void SetActiveKeyboard(string properties)
        {
            if (properties != null)
            {
                activeKeyboard = Keyboards.Find(x => x.GetDeviceProperties() == properties);
            }
            else
            {
                activeKeyboard = null;
            }


            App.Current.Dispatcher.Invoke(() =>
            {
                if (activeKeyboard != null)
                {
                    prefix = activeKeyboard.QMKStringPrefix;
                    suffix = activeKeyboard.QMKStringSuffix;
                    CreateQMKKeyDictionary();
                    overlay.ActivateKeyboard(activeKeyboard);
                }

            });

        }
        */
        public void ShowOverlay()
        {
            overlay.Show();
        }

        public void HideOverlay()
        {
            overlay.Hide();
        }

        public void ReadQMKOutput(string QMKString)
        {
            if (activeKeyboard == null)
            {
                return;
            }

            string StringType = QMKString.Substring(0, 1);
            string data = QMKString.Substring(1, QMKString.Length - 1);

            string key = QMKOutputTranslation.First(x => x.Key == StringType).Value;
            switch (key)
            {
                case "Layer":
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        overlay.UpdateLayer(data);
                    });
                    break;
                case "Key":
                    if(miTrackKeys.Checked)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            overlay.UpdateKeyPress(data);
                        });
                    }

                    break;
                case "CAPS":
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        overlay.UpdateCaps(data);
                    });
                    break;
                case "Macro":
                    ProcessMacro(data);
                    break;
                case "Indicator":
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        overlay.UpdateIndicator(data);
                    });
                    break;
                case "QMK":
                    ProcessQMKRequest(data);
                    break;
            }
        }

        Dictionary<string, string> QMKKeyTranslation = new Dictionary<string, string>();
        Dictionary<string, string> QMKOutputTranslation = new Dictionary<string, string>();

        private void CreateQMKKeyDictionary()
        {
            QMKKeyTranslation.Clear();
            QMKKeyTranslation.Add(activeKeyboard.QMKOpacityDown, "OpacityDown");
            QMKKeyTranslation.Add(activeKeyboard.QMKOpacityUp, "OpacityUp");
            QMKKeyTranslation.Add(activeKeyboard.QMKTypeDown, "DisplayDown");
            QMKKeyTranslation.Add(activeKeyboard.QMKTypeUp, "DisplayUp");
            QMKKeyTranslation.Add(activeKeyboard.QMKSizeDown, "SizeDown");
            QMKKeyTranslation.Add(activeKeyboard.QMKSizeUp, "SizeUp");
            QMKKeyTranslation.Add(activeKeyboard.QMKPositionDown, "PositionDown");
            QMKKeyTranslation.Add(activeKeyboard.QMKPositionUp, "PositionUp");
            QMKKeyTranslation.Add(activeKeyboard.QMKKeyboardDown, "KeyboardDown");
            QMKKeyTranslation.Add(activeKeyboard.QMKKeyboardUp, "KeyboardUp");
            QMKKeyTranslation.Add(activeKeyboard.QMKMonitor, "QMKMonitor");
            //QMKKeyTranslation.Add(activeKeyboard.QMKSymbolHelp, "HelpSymbol");
            //QMKKeyTranslation.Add(activeKeyboard.QMKMacroHelp, "HelpMacro");

            QMKOutputTranslation.Clear();
            QMKOutputTranslation.Add(activeKeyboard.QMKSave, "SaveSettings");
            QMKOutputTranslation.Add(activeKeyboard.QMKLayerCode, "Layer");
            QMKOutputTranslation.Add(activeKeyboard.QMKKeystrokeCode, "Key");
            QMKOutputTranslation.Add(activeKeyboard.QMKIndicatorCode, "Indicator");
            QMKOutputTranslation.Add(activeKeyboard.QMKCapsCode, "CAPS");
            QMKOutputTranslation.Add(activeKeyboard.QMKMacroCode, "Macro");
            QMKOutputTranslation.Add(activeKeyboard.QMKQMKKeyCode, "QMK");
        }
        private void ProcessQMKRequest(string data)
        {
            string key = QMKKeyTranslation.First(x => x.Key == data).Value;

            MenuItem mi = new MenuItem();
            int ix = 0;
            int step = 0;
            switch (key)
            {
                case "OpacityDown":
                    mi = miOverlayOpacity;
                    ix = OverlayOpacityIndex;
                    step = -1;
                    break;
                case "OpacityUp":
                    mi = miOverlayOpacity;
                    ix = OverlayOpacityIndex;
                    step = 1;
                    break;
                case "DisplayDown":
                    mi = miOverlayDisplay;
                    ix = OverlayTypeIndex;
                    step = -1;
                    break;
                case "DisplayUp":
                    mi = miOverlayDisplay;
                    ix = OverlayTypeIndex;
                    step = 1;
                    break;
                case "SizeDown":
                    mi = miOverlaySize;
                    ix = OverlaySizeIndex;
                    step = -1;
                    break;
                case "SizeUp":
                    mi = miOverlaySize;
                    ix = OverlaySizeIndex;
                    step = 1;
                    break;
                case "PositionDown":
                    mi = miOverlayPosition;
                    ix = OverlayPositionIndex;
                    step = -1;
                    break;
                case "PositionUp":
                    mi = miOverlayPosition;
                    ix = OverlayPositionIndex;
                    step = 1;
                    break;
                case "KeyboardDown":
                    mi = miKeyboardOverlays;
                    ix = KeyboardIndex;
                    step = -1;
                    break;
                case "KeyboardUp":
                    mi = miKeyboardOverlays;
                    ix = KeyboardIndex;
                    step = 1;
                    break;
                case "SaveSettings":
                    SaveOverlaySettings();
                    break;
                case "Monitor":
                    miTrackKeys.PerformClick();
                    break;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                NavigateMenus(mi, ix, step);
            });
        }

        private void NavigateMenus(MenuItem mi, int start, int increment)
        {
            int n = mi.MenuItems.Count;
            mi.MenuItems[(start + n + increment) % n].PerformClick();
        }


        #endregion
        #region Macro

        private void ProcessMacro(string code)
        {
            KeyboardMacro m = activeKeyboard.Macros.Find(x => x.Layer == int.Parse(code.Substring(0, 1), System.Globalization.NumberStyles.HexNumber) &&
                                           x.Column == int.Parse(code.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) &&
                                           x.Row == int.Parse(code.Substring(3, 2), System.Globalization.NumberStyles.HexNumber));

            if(m != null)
            {
                if(m.MacroType == MacroType.KeyPress)
                {
                    SendKeys.SendWait(m.MacroText);
                }
                else
                {
                    
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        System.Windows.Clipboard.SetText(m.MacroText);
                    });
                    SendKeys.SendWait("^v");
                }
                
            }
            
        }

        #endregion
    }
}
