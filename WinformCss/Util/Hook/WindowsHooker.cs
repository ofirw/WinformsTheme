using System;
using System.Runtime.InteropServices;

namespace WinformsTheme.Util.Hook
{
    public class WindowsHooker : IDisposable
    {
        private Action<CWPRETSTRUCT> hookAction;
        private UnsafeNativeMethods.HookProc hookProc;
        private IntPtr hook;

        internal static WindowsHooker Hook(Action<CWPRETSTRUCT> on_WH_CALLWNDPROCRET)
        {
            var hooker = new WindowsHooker();
            hooker.HookAction(on_WH_CALLWNDPROCRET);

            return hooker;
        }

        private void HookAction(Action<CWPRETSTRUCT> action)
        {
            this.hookAction = action;
            this.hookProc = new UnsafeNativeMethods.HookProc(MessageHook);
            this.hook = UnsafeNativeMethods.SetWindowsHookEx(
                HookType.WH_CALLWNDPROCRET,
                this.hookProc,
                IntPtr.Zero,
                UnsafeNativeMethods.GetCurrentThreadId());
        }

        private IntPtr MessageHook(int code, IntPtr wParam, IntPtr lParam)
        {
            var m = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));

            this.hookAction(m);

            return UnsafeNativeMethods.CallNextHookEx(this.hook, code, wParam, lParam);
        }

        #region Dispose Pattern
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                if (this.hook != IntPtr.Zero)
                {
                    UnsafeNativeMethods.UnhookWindowsHookEx(this.hook);
                    this.hook = IntPtr.Zero;
                }

                this.disposedValue = true;
            }
        }

        ~WindowsHooker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose Pattern
    }
}
