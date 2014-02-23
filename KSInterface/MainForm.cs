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

namespace KSInterface
{
    public partial class MainForm : Form
    {
        private bool hooked = false;

        public MainForm()
        {
            InitializeComponent();
            Log("Initialized");
        }

        private void Log(string text, string colorName = "Black")
        {
            rtbLog.SelectionColor = Color.FromName(colorName);
            rtbLog.AppendText(DateTime.Now.ToString("HH:mm:ss")+ ":  ");
            rtbLog.AppendText(text + "\n");
            rtbLog.ScrollToCaret();
            rtbLog.Update();
        }
        private void MyCallback(int code, UInt32 wparam, Int32 lparam)
        {
            Log("MyCallback");
        }
        private void Start_Click(object sender, EventArgs e)
        {
            if (hooked)
            {
                Log("Already hooked");
            }
            else
            {
                KSDllWrapper.InstallHook(new KSDllWrapper.Callback(MyCallback));
                hooked = true;
                Log("Started");
            }
        }

        private void End_Click(object sender, EventArgs e)
        {
            if (hooked)
            {
                Log("Terminated ");
                KSDllWrapper.UninstallHook();
                hooked = false;
            }
            else
            {
                Log("Not hooked");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hooked)
            {
                KSDllWrapper.UninstallHook();
            }
        }

    }
}
