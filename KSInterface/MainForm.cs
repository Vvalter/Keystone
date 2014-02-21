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
        [DllImport("KSDll.dll", EntryPoint = "KSDLLadd", CallingConvention = CallingConvention.Cdecl)]
        public static extern int KSDLLadd(int a, int b);

        [DllImport("KSDll.dll", EntryPoint = "SetKeyboardCallback", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr SetKeyboardCallback(HookCallback hc);

        [DllImport("KSDll.dll", EntryPoint = "RemoveHook", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool RemoveHook();

        public delegate void HookCallback(int code, UIntPtr wparam, IntPtr lparam);

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
            Log("Initialized");
        }

        int i = 0;
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void KeyboardEvent(int code, UIntPtr wparam, IntPtr lparam)
        {
            try
            {
                i++;
                //System.Console.Write((i++).ToString() + '\n');
                //Log((i++).ToString());
            }
            catch (Exception) { }
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
                HookCallback hc = new HookCallback(KeyboardEvent);
                GarbageCollectionSucks.SaveCallback(hc);
                SetKeyboardCallback(hc);
                hooked = true;
                Log("Started");
            }
        }

        private void End_Click(object sender, EventArgs e)
        {
            if (hooked)
            {
                Log("Terminated " + RemoveHook().ToString());
                Log("i: " + i.ToString());
                hooked = false;
            }
            else
            {
                Log("Not hooked");
            }
        }
    }
    public static class GarbageCollectionSucks
    {
        private static MainForm.HookCallback save;
        public static void SaveCallback(MainForm.HookCallback hc)
        {
            save = hc;
        }
    }
}
