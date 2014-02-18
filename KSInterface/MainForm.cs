using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSInterface
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Log("Initialized\n");
        }

        private void Log(string text, string colorName = "Black")
        {
            rtbLog.SelectionColor = Color.FromName(colorName);
            rtbLog.AppendText(DateTime.Now.ToString("HH:mm:ss")+ ":  ");
            rtbLog.AppendText(text);
            rtbLog.ScrollToCaret();
            rtbLog.Update();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Log("Started\n");
        }

        private void End_Click(object sender, EventArgs e)
        {
            Log("Terminated\n");
        }
    }
}
