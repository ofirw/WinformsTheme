using System;
using System.Runtime.InteropServices;

namespace WinformsTheme.Util.Hook
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CWPRETSTRUCT
    {
        public IntPtr lResult;
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hWnd;

        public WindowMessage GetMessage() => (WindowMessage)this.message;
    }
}
