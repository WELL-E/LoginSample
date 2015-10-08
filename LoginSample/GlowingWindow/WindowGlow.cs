using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using GlowingWindow;

namespace GlowingWindow
{
    public class WindowGlow
    {
        #region [private]

        private Window _parentWindow;
        private IntPtr _parentWindowHndl;
        private SideGlow _topGlow;
        private SideGlow _leftGlow;
        private SideGlow _bottomGlow;
        private SideGlow _rightGlow;
        private HwndSource _source;
        private Color _activeColor = Colors.Gray;
        private Color _inactiveColor = Colors.Gray;
        private bool _isFocused;

        #endregion

        #region [public]

        public Color ActiveColor
        {
            get
            {
                return _activeColor;
            }

            set
            {
                _activeColor = value;
                UpdateColor();
            }
        }

        public Color InactiveColor
        {
            get
            {
                return _inactiveColor;
            }

            set
            {
                _inactiveColor = value;
                UpdateColor();
            }
        }

        #endregion

        #region Constructor

        public WindowGlow()
        {
            _topGlow = new SideGlow(Side.Top);
            _leftGlow = new SideGlow(Side.Left);
            _bottomGlow = new SideGlow(Side.Bottom);
            _rightGlow = new SideGlow(Side.Right);
        }

        #endregion

        #region [public] API

        public void Attach(Window window)
        {
            _parentWindow = window;
            _parentWindowHndl = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_parentWindowHndl);
            Debug.Assert(_source != null);
            _source.AddHook(ParentWndProc);

            _topGlow.Show();
            _bottomGlow.Show();
            _leftGlow.Show();
            _rightGlow.Show();
        }

        public void Detach()
        {
            _topGlow.Close();
            _bottomGlow.Close();
            _leftGlow.Close();
            _rightGlow.Close();

            _parentWindow = null;
            _source.RemoveHook(ParentWndProc);
            _source.Dispose();
            _source = null;
        }

        public void Hide()
        {
            _topGlow.Hide();
            _bottomGlow.Hide();
            _leftGlow.Hide();
            _rightGlow.Hide();
        }

        public void Show()
        {
            _topGlow.Show();
            _bottomGlow.Show();
            _leftGlow.Show();
            _rightGlow.Show();
        }

        #endregion

        #region [private]

        private IntPtr ParentWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg.Is(WindowsMessage.WM_ACTIVATE))
            {
                UpdateZOrder();
            }
            else if (msg.Is(WindowsMessage.WM_WINDOWPOSCHANGED))
            {
                WindowsAPI.WINDOWPOS pos = (WindowsAPI.WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WindowsAPI.WINDOWPOS));
                UpdateLocations(pos);
            }
            else if (msg.Is(WindowsMessage.WM_SIZE))
            {
                if ((int)wParam == 2 || (int)wParam == 1) // maximized/minimized
                {
                }
                else
                {
                    int width = (int)WindowsAPI.LoWord(lParam);
                    int height = (int)WindowsAPI.HiWord(lParam);
                    UpdateSizes(width, height);
                }
            }
            else if (msg.Is(WindowsMessage.WM_DESTROY))
            {

            }
            else if (msg.Is(WindowsMessage.WM_SETFOCUS))
            {
                _isFocused = true;
                UpdateZOrder();
                UpdateColor();
            }
            else if (msg.Is(WindowsMessage.WM_KILLFOCUS))
            {
                _isFocused = false;
                UpdateColor();
            }

            return IntPtr.Zero;
        }

        private void UpdateColor()
        {
            Color c = _inactiveColor;
            if (_isFocused)
            {
                c = _activeColor;
            }

            _topGlow.Color = c;
            _leftGlow.Color = c;
            _rightGlow.Color = c;
            _bottomGlow.Color = c;
        }

        private void UpdateSizes(int width, int height)
        {
            _topGlow.SetSize(width);
            _bottomGlow.SetSize(width);
            _leftGlow.SetSize(height);
            _rightGlow.SetSize(height);
        }

        private void UpdateLocations(WindowsAPI.WINDOWPOS pos)
        {
            _topGlow.SetLocation(pos);
            _bottomGlow.SetLocation(pos);
            _rightGlow.SetLocation(pos);
            _leftGlow.SetLocation(pos);
        }

        private void UpdateZOrder()
        {
            UpdateWindowZOrder(_topGlow.Handle);
            UpdateWindowZOrder(_bottomGlow.Handle);
            UpdateWindowZOrder(_leftGlow.Handle);
            UpdateWindowZOrder(_rightGlow.Handle);
        }

        private void UpdateWindowZOrder(IntPtr handle)
        {
            //WindowsAPI.SetWindowPos(_parentWindowHndl, 0, 0, 0, 0, 9, WindowsAPI.SWP_NOMOVE | WindowsAPI.SWP_NOSIZE | WindowsAPI.SWP_NOACTIVATE);
            WindowsAPI.SetWindowPos(handle, _parentWindowHndl.ToInt32(), 0, 0, 0, 9, WindowsAPI.SWP_NOMOVE | WindowsAPI.SWP_NOSIZE | WindowsAPI.SWP_NOACTIVATE);
        }

        #endregion
    }
}
