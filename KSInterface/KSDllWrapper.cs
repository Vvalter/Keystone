using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Concurrent;
namespace KSInterface
{
    static class KSDllWrapper
    {
        private delegate void Callback(bool down, int vkc);
        private static Callback _func = new Callback(HookFunc); // Save it from GC
        // TODO Why is init neccessary (not static constructor)
        public static void init()
        {
            KeyInputs = new BlockingCollection<Keys>(new ConcurrentQueue<Keys>());
            InstallHook(_func);
        }

        #region Keyboard Hook

        /* Keyboard Hook */

        public static BlockingCollection<Keys> KeyInputs;
        private static void HookFunc(bool down, int vkc)
        {
            if (!down)
            {
                KeyInputs.Add((Keys)vkc);
            }
        }

        [DllImport("KSDll.dll", EntryPoint="InstallHook", CallingConvention = CallingConvention.Cdecl, SetLastError=true)]
        private static extern IntPtr _InstallHook(Callback c);
        private static void InstallHook(Callback c)
        {
            IntPtr hook = _InstallHook(c);
            if (hook == IntPtr.Zero)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
        }

        [DllImport("KSDll.dll", EntryPoint = "UninstallHook", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool _UninstallHook();
        private static void UninstallHook()
        {
            bool success = _UninstallHook();

            if (!success)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
        }

        /* Input Managment */


        [DllImport("KSDll.dll", EntryPoint = "SetMousePosition", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void SetMousePosition(int x, int y);

        [DllImport("KSDll.dll", EntryPoint = "MoveMouse", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void MoveMouse(int dx, int dy);

        [DllImport("KSDll.dll", EntryPoint = "PressMouse", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void PressMouse(bool down);


        /* Window Managment */
        
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
            public long x, y;
            public long width, height;
            
            public Rectangle(long x, long y, long width, long height)
            {
                this.x = x;
                this.y = y;

                this.width = width;
                this.height = height;
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
        #endregion
    }
}
