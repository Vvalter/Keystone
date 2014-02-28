using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Drawing;

namespace KSInterface
{
    public enum ASPECTRATIO
    {
        R16_9, R16_10, R4_3
    }

    static class KSDllWrapper
    {
        private delegate void Callback(bool down, int vkc);
        private static Callback _func = new Callback(KeyboardHookFunc); // Save it from GC
        public static void init()
        {
            KeyInputs = new BlockingCollection<Keys>(new ConcurrentQueue<Keys>());
            InstallHook(HOOKID.WH_KEYBOARD_LL, _func);
        }

        #region Windows Hooks

        /* Keyboard Hook */
        public static BlockingCollection<Keys> KeyInputs;
        private static void KeyboardHookFunc(bool down, int vkc)
        {
            if (!down)
            {
                KeyInputs.Add((Keys)vkc);
            }
        }

        /* General Hooks */
        private enum HOOKID {
            WH_CBT = 5,
            WH_KEYBOARD_LL = 13            
        };

        [DllImport("KSDll.dll", EntryPoint="InstallHook", CallingConvention = CallingConvention.Cdecl, SetLastError=true)]
        private static extern IntPtr _InstallHook(HOOKID id, Callback c);
        private static void InstallHook(HOOKID id, Callback c)
        {
            IntPtr hook = _InstallHook(id, c);
            if (hook == IntPtr.Zero)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
        }

        [DllImport("KSDll.dll", EntryPoint = "UninstallHook", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool _UninstallHook(HOOKID id);
        private static void UninstallHook(HOOKID id)
        {
            bool success = _UninstallHook(id);

            if (!success)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
        }

        #endregion

        #region Mouse Movement

        [DllImport("KSDll.dll", EntryPoint = "SetMousePosition", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void SetMousePosition(int x, int y);

        [DllImport("KSDll.dll", EntryPoint = "MoveMouse", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void MoveMouse(int dx, int dy);

        [DllImport("KSDll.dll", EntryPoint = "PressMouse", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void PressMouse(bool down);

        #endregion

        #region Window Managment
        [DllImport("User32.dll")]
        private static extern IntPtr FindWindowW([MarshalAs(UnmanagedType.LPWStr)] string className, [MarshalAs(UnmanagedType.LPWStr)] string windowName);

        [DllImport("KSDll.dll", EntryPoint = "GetWindowX", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetWindowX(IntPtr hWnd);

        [DllImport("KSDll.dll", EntryPoint = "GetWindowY", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetWindowY(IntPtr hWnd);

        [DllImport("KSDll.dll", EntryPoint = "GetWindowWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetWindowWidth(IntPtr hWnd);

        [DllImport("KSDll.dll", EntryPoint = "GetWindowHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetWindowHeight(IntPtr hWnd);

        public struct Rectangle
        {
            public int x, y;
            public int width, height;

            public Rectangle(long x, long y, long width, long height)
            {
                this.x = (int)x;
                this.y = (int)y;

                this.width = (int)width;
                this.height = (int)height;
            }
        }

        public static Rectangle GetHearthstoneWindow()
        {
            IntPtr hWnd = FindWindowW(null, "Hearthstone");
            if (hWnd == IntPtr.Zero)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
            return new Rectangle(GetWindowX(hWnd)+3, GetWindowY(hWnd)+25, GetWindowWidth(hWnd), GetWindowHeight(hWnd));
        }

        
        public static ASPECTRATIO GetAspectRatio()
        {
            Rectangle r = GetHearthstoneWindow();
            double tmp = r.width / 16.0;
            switch ((int) Math.Round(r.height / tmp))
            { 
                case 9:
                    return ASPECTRATIO.R16_9;
                case 10:
                    return ASPECTRATIO.R16_10;
                case 12:
                    return ASPECTRATIO.R4_3;
                default:
                    throw new Exception("Invalid Aspect Ratio found resolution: " + GetResolution());
            }
        }

        public static string GetResolution()
        {
            Rectangle r = GetHearthstoneWindow();

            if (r.height == 1000)
            {
                if (r.width == 1600)
                {
                    return "1600x1024";
                }

                if (r.width == 1680)
                {
                    return "1680x1050";
                }

                if (r.width == 1840)
                {
                    return "1920x1080";
                }
                return "error";
            }

            return r.width.ToString() + "x" + r.height.ToString();
        }

        #endregion
    }
}
