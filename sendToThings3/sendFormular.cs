﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sendToThings3
{
    public partial class sendFormular : Form
    {
        KeyboardHook hook = new KeyboardHook();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );


        public sendFormular()
        {
            InitializeComponent();
            // this.FormBorderStyle = FormBorderStyle.None;
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 10, 10));

            // register the event that is fired after the key press.
            hook.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey(sendToThings3.ModifierKeys.Control | sendToThings3.ModifierKeys.Alt,
                Keys.OemPeriod);

            p_text.Visible = true;
            p_message.Enabled = false;
            p_message.Visible = false;

        }

        //override onshow
        protected override void OnShown(EventArgs e)
        {
            tb_title.Select();
        }


        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.
            if (this.Visible)
            {
                this.Hide();
                return;
            }

            this.Show();
            tb_title.Select();

        }



        /**
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }
        **/


        private void sendFormular_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control || e.KeyCode == Keys.Enter && e.Control)
            {
                saveNote();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                cancelNote();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void sendFormular_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            { 
                p_text.Visible = false;
                p_message.Enabled = true;
                p_message.Visible = true;
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            saveNote();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            cancelNote();
        }

        private void saveNote()
        {
            if (!string.IsNullOrEmpty(tb_title.Text))
            {
                if ((tb_message.TextLength + tb_title.TextLength) < 2000)
                {
                    
                    this.Hide();

                    if (Utils.SendMail(tb_title.Text, tb_message.Text)) 
                    {
                        tb_title.Clear();
                        tb_message.Clear();
                    }
                    else
                    {
                        this.Show();
                        MessageBox.Show("Something went wrong sending the mail");
                    }

                    p_text.Visible = true;
                    p_message.Enabled = false;
                    p_message.Visible = false;

                }
                else
                {
                    MessageBox.Show($"Stay under 2000 characters\nRemove {(tb_message.TextLength + tb_title.TextLength) - 2001} characters");
                }

            }
        }

        private void cancelNote()
        {
            tb_title.Clear();
            tb_message.Clear();
            this.Hide();

            p_text.Visible = true;
            p_message.Enabled = false;
            p_message.Visible = false;
        }

    }
}
