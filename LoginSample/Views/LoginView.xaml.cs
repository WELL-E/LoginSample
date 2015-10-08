using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using GlowingWindow;
using LoginSample.Hepler;

namespace LoginSample.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        private WindowGlow _glow;

        public LoginView()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        private void LoginView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsHepler.SetWindowNoBorder(hwnd);

            _glow = new WindowGlow();
            _glow.Attach(this);
            _glow.ActiveColor = Colors.Red;
            _glow.InactiveColor = Colors.Gray;
        }

        private void LoginView_OnClosed(object sender, EventArgs e)
        {
            if (_glow != null)
            {
                _glow.Detach();
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ChkRemeberPwd_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ChkAutoLogin_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LinkReg_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LinRecoverPwd_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PbPwd_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Min_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
