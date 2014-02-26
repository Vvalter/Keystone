using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace KSInterface
{
    public partial class MainForm : Form
    {
        private KeyboardController _controller;
        private Thread t;
        private bool running = false;
        public MainForm()
        {
            this.TopMost = true;
            KSDllWrapper.init();
            InitializeComponent();
        }

        private delegate void LogCallback(string text, string colorName);
        public void Log(string text, string colorName = "Black")
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new LogCallback(Log), new object[]{text, colorName});
                return;
            }
            rtbLog.SelectionColor = Color.FromName(colorName);
            rtbLog.AppendText(DateTime.Now.ToString("HH:mm:ss")+ ":  ");
            rtbLog.AppendText(text + "\n");
            rtbLog.ScrollToCaret();
            rtbLog.Update();
        }
        private void Start_Click(object sender, EventArgs e)
        {
            try
            {
                KSDllWrapper.Rectangle r = KSDllWrapper.GetHearthstoneWindow();
                Log(String.Format("x:{0} y:{1} width:{2} height:{3}", r.x, r.y, r.width, r.height));
            } 
            catch (Win32Exception)
            {
                Log("Hearthstone not running!");
            }
            return;
            if (running)
            {
                Log("Already running");
                return;
            }
            _controller = new KeyboardController(this, 0, 0, 1920, 1080);
            t = new Thread(_controller.KeyboardLoop);
            t.Name = "Controller Thread";
            t.IsBackground = true;
            t.Start();

            running = true;
        }

        private void End_Click(object sender, EventArgs e)
        {
            if (running)
            {
                Log("Ended");
            }
            else
            {
                Log("Not running");
            }
            t.Abort();
            _controller = null;
            running = false;
        }
    }
}
