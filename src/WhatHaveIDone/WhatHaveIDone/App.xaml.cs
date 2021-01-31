using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using WhatHaveIDone.Core.Persistence;

[assembly:InternalsVisibleTo("WhatHaveIDone.Test")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace WhatHaveIDone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        private const string Close = "Close";
        private const string Show = "Show";
        private NotifyIcon _systemTrayIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            IoCConfiguration.ConfigureDependencies();

            CreateSystemTray();
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }

        private void CloseApp()
        {
            _systemTrayIcon.Visible = false;

            this.Shutdown(0);
        }

        private ContextMenuStrip CreateContextMenuForSystemTray()
        {
            var menu = new ContextMenuStrip();

            menu.Items.Add(Show);
            menu.Items.Add(Close);

            menu.Click += SystemTrayContextMenu_Click;
            return menu;
        }

        private void CreateSystemTray()
        {
            _systemTrayIcon = new NotifyIcon();
            _systemTrayIcon.Icon = new System.Drawing.Icon("./Assets/TrayIcon.ico");
            _systemTrayIcon.Visible = true;
            _systemTrayIcon.Text = "What have I done?";
            _systemTrayIcon.DoubleClick += SystemTrayIcon_DoubleClick;
            _systemTrayIcon.ContextMenuStrip = CreateContextMenuForSystemTray();
        }

        private void ShowApp()
        {
            this.MainWindow.Show();
        }

        private void SystemTrayContextMenu_Click(object sender, System.EventArgs e)
        {
            var menu = (ContextMenuStrip)sender;
            var mouseEvent = (MouseEventArgs)e;

            var clickedItem = menu.GetItemAt(mouseEvent.X, mouseEvent.Y);

            if (clickedItem.Text == Close)
            {
                CloseApp();
            }

            if (clickedItem.Text == Show)
            {
                ShowApp();
            }
        }

        private void SystemTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowApp();
        }
    }
}