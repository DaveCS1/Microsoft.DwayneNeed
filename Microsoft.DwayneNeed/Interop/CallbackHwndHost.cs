using System;
using Microsoft.DwayneNeed.Win32.User32;

namespace Microsoft.DwayneNeed.Interop
{
    /// <summary>
    ///     A simple HwndHost that accepts callbacks for creating and
    ///     destroying the hosted window.
    /// </summary>
    public class CallbackHwndHost : HwndHostEx
    {
        private readonly Func<HWND, HWND> _buildWindow;
        private readonly Action<HWND> _destroyWindow;

        public CallbackHwndHost(Func<HWND, HWND> buildWindow, Action<HWND> destroyWindow)
        {
            _buildWindow = buildWindow;
            _destroyWindow = destroyWindow;
        }

        protected override HWND BuildWindowOverride(HWND hwndParent)
        {
            return _buildWindow(hwndParent);
        }

        protected override void DestroyWindowOverride(HWND hwnd)
        {
            _destroyWindow(hwnd);
        }
    }
}