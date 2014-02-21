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
        [DllImport("KSDll.dll", EntryPoint="_KSDLLadd@8")]
        public static extern int KSDLLadd(int a, int b);

        [DllImport("KSDll.dll", EntryPoint = "_SetKeyboardCallback@4")]
        private static extern IntPtr SetKeyboardCallback(HookCallback hc);

        private delegate void HookCallback(int code, UIntPtr wparam, IntPtr lparam);
        private HookCallback hc = null;// = new HookCallback(KeyboardEvent);
        private bool hooked = false;

        private enum HookID
        {
            JournalRecord = 0,
            JournalPlayback = 1,
            KeyboardLL = 13,
            MouseLL = 14
        }

        public MainForm()
        {
            InitializeComponent();
            hc = new HookCallback(KeyboardEvent);

            Log("Initialized");
        }

        int i = 0;
        //[MethodImpl(MethodImplOptions.NoInlining)]
        private void KeyboardEvent(int code, UIntPtr wparam, IntPtr lparam)
        {
            System.Console.Write((i++).ToString() + '\n');
            /*try
            {
                Log("KeyboardEvent");
                Log(code.ToString());
            } catch (Exception e) {}*/
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
            if (hooked)
            {
                Log("Already hooked");
            }
            else
            {
                SetKeyboardCallback(new HookCallback(KeyboardEvent));
                hooked = true;
                Log("Started");
            }
        }

        private void End_Click(object sender, EventArgs e)
        {
            if (hooked)
            {
                Log("Terminated");
                Log(KSDLLadd(0, 0).ToString());
                hooked = false;
            }
            else
            {
                Log("Not hooked");
            }
        }
    }
}
