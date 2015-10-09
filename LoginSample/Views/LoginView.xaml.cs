using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using GlowingWindow;
using LoginSample.Hepler;
using LoginSample.ViewModels;

namespace LoginSample.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView
    {
        private WindowGlow _glow;

        public LoginView()
        {
            InitializeComponent();

            this.DataContext = new LoginViewModel(LoginViewClose);
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

        private void LoginViewClose()
        {
            if (_glow != null)
            {
                _glow.Detach();
            }

            this.Close();
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
            Process.Start("https://github.com/WELL-E");
        }

        private void LinRecoverPwd_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/WELL-E");
        }

        private void PbPwd_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordtext = (PasswordBox)sender;
            SetPasswordBoxSelection(passwordtext, passwordtext.Password.Length + 1, passwordtext.Password.Length + 1);
        }

        private static void SetPasswordBoxSelection(PasswordBox passwordBox, int start, int length)
        {
            var select = passwordBox.GetType().GetMethod("Select",
                BindingFlags.Instance | BindingFlags.NonPublic);

            select.Invoke(passwordBox, new object[] { start, length });
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            LoginViewClose();
        }

        private void Min_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
