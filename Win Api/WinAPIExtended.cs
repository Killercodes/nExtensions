using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace nExtensions
{
    public static class WinAPIExtended
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow ();

        [DllImport("user32.dll")]
        static extern int GetWindowText (IntPtr hWnd, StringBuilder text, int count);
        public static string GetActiveWindowTitle ()
        {
            IntPtr handle = IntPtr.Zero;
            StringBuilder title = new StringBuilder(256);
            handle = GetForegroundWindow();
            if (handle != IntPtr.Zero)
            {
                if (GetWindowText(handle, title, 256) > 0)
                    return title.ToString();
            }
            return "not found";
        }

    }
}
