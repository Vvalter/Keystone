using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace KSInterface
{
    static class KSDllWrapper
    {
        #region Keyboard Hook

        public delegate void Callback(int code, UInt32 wparam, Int32 lparam);

        [DllImport("KSDll.dll", EntryPoint="InstallHook", CallingConvention = CallingConvention.Cdecl, SetLastError=true)]
        private static extern IntPtr _InstallHook(Callback c);

        public static void InstallHook(Callback c)
        {
            IntPtr hook = _InstallHook(c);
            System.Console.Write(hook.ToString());
            if (hook == IntPtr.Zero)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
        }

        [DllImport("KSDll.dll", EntryPoint = "UninstallHook", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern bool _UninstallHook();

        public static void UninstallHook()
        {
            bool success = _UninstallHook();

            if (!success)
            {
                Int32 err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }
        }

        #endregion

        #region Board Managment

        [DllImport("KSDll.dll", EntryPoint = "SetNumEnemy", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetNumEnemy(int num);

        [DllImport("KSDll.dll", EntryPoint = "SetNumFriendly", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetNumFriendly(int num);

        [DllImport("KSDll.dll", EntryPoint = "SetNumCards", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void SetNumCards(int num);

        #endregion

        // TODO Implement other functions
    }
}
