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
        public static MainForm mf;
        private KeyboardController _controller;
        private bool _running = false;

        private delegate void LogCallback(string text, string colorName);
        private delegate void CardCallback(int cards);
        public MainForm()
        {
            mf = this;
            this.TopMost = true;
            KSDllWrapper.init();
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
        private delegate void ImageCallback(Image img);
	public void SetImage(Image img)
        {
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new ImageCallback(SetImage), new object[] { img });
                return;
            }
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            pictureBox1.Image = img;
        }
        private void Start_Click(object sender, EventArgs e)
        {
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

            _controller = new KeyboardController(this);
            _running = true;
        }
        private void End_Click(object sender, EventArgs e)
        {
            if (_running)
            {
                Log("Ended");
		// TODO
                _running = false;
                _controller.Stop();
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
