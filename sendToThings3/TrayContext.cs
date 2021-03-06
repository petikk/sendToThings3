﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sendToThings3.Properties;

namespace sendToThings3
{
    class TrayContext : ApplicationContext
    {
        private SendForm _sendForm = new SendForm();

        private readonly NotifyIcon _trayIcon;

        public TrayContext()
        {
            // Initialize Tray Icon
            _trayIcon = new NotifyIcon()
            {
                Icon = Resources.logo ,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("New Task", New),
                    new MenuItem("Settings", Settings),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            Application.Exit();
        }

        void New(object sender, EventArgs e)
        {
            _sendForm.Show();
        }

        void Settings(object sender, EventArgs e)
        {
            new Settings().Show();
        }
    }
}
