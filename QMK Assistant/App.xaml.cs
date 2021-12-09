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
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        public static Assistant assistant = new Assistant();

        public static NotifyIcon notifyIcon;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = QMK_Assistant.Properties.Resources.MCU_White;
            notifyIcon.Visible = true;

            assistant.Start();

        }




    }
}
