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
        private Thread _controllerThread;
        private bool _running = false;

        private delegate void LogCallback(string text, string colorName);
        private delegate void CardCallback(int cards);
        public MainForm()
        {
            this.TopMost = true;
            KSDllWrapper.init();
            _controller = new KeyboardController(this);
            InitializeComponent();
        }
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
        public void SetEnemyCards(int cards)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new CardCallback(SetEnemyCards), new object[] { cards });
                return;
            }
            textBox1.Text = cards.ToString();
            textBox1.Update();
        }
        public void SetFriendlyCards(int cards)
        {
            if (textBox2.InvokeRequired)
            {
                textBox2.Invoke(new CardCallback(SetFriendlyCards), new object[] { cards });
                return;
            }
            textBox2.Text = cards.ToString();
            textBox2.Update();
        }
        private void Start_Click(object sender, EventArgs e)
        {
            ImageHelper.GetBitmap();
            if (_running)
            {
                Log("Already running", "red");
                return;
            }
            try
            {
                Log(String.Format("Found Hearthstone Widow at {0}x{1} with " + KSDllWrapper.GetResolution(), KSDllWrapper.GetHearthstoneWindow().x, KSDllWrapper.GetHearthstoneWindow().y));
            }
            catch (Win32Exception)
            {
                Log("Unable to find Hearthstone", "red");
                return;
            }

            if (!_controller.Update())
            {
                Log("Unable to find Hearthstone", "red");
                return;
            }

            _controllerThread = new Thread(_controller.KeyboardLoop);
            _controllerThread.Name = "Controller Thread";
            _controllerThread.IsBackground = true;
            _controllerThread.Start();

            _running = true;
        }
        private void End_Click(object sender, EventArgs e)
        {
            if (_running)
            {
                Log("Ended");
                _controllerThread.Abort();
                _running = false;
            }
            else
            {
                Log("Not running", "red");
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (_controller.Update())
            {
                Log("Updated");
            }
            else
            {
                Log("Update failed", "red");
            }
        }
    }
}
