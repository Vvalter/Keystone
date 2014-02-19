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

namespace KSInterface
{
    public partial class MainForm : Form
    {
        [DllImport("KSDll.dll", EntryPoint="#1")]
        public static extern int KSDLLadd(int a, int b);

        public MainForm()
        {
            InitializeComponent();
            Log("Initialized");
            Log("1+1: " + KSDLLadd(1, 1));       
        }

        private void Log(string text, string colorName = "Black")
        {
            rtbLog.SelectionColor = Color.FromName(colorName);
            rtbLog.AppendText(DateTime.Now.ToString("HH:mm:ss")+ ":  ");
            rtbLog.AppendText(text + "\n");
            rtbLog.ScrollToCaret();
            rtbLog.Update();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Log("Started");
        }

        private void End_Click(object sender, EventArgs e)
        {
            Log("Terminated");
        }

        private void rtbLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
