using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using LoginSample.Comm;
using LoginSample.Models;

namespace LoginSample.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region 字段 属性
        private const string LoginString = "登录";
        private const string CancelString = "取消";

        /// <summary>
        /// //关闭窗口
        /// </summary>
        private readonly Action _closeAction;

        /// <summary>
        /// 登录线程
        /// </summary>
        private readonly BackgroundWorker _bkWorker;

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string _userPwd;

        public string UserPwd
        {
            get { return _userPwd; }
            set
            {
                _userPwd = value;
                OnPropertyChanged("UserPwd");
            }
        }

        /// <summary>
        /// 是否记住密码
        /// </summary>
        private bool _isRememberPwd;

        public bool IsRememberPwd
        {
            get { return _isRememberPwd; }
            set
            {
                _isRememberPwd = value;
                OnPropertyChanged("IsRememberPwd");
            }
        }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        private bool _isAutoLogin = false;

        public bool IsAutoLogin
        {
            get { return _isAutoLogin; }
            set
            {
                _isAutoLogin = value;
                OnPropertyChanged("IsAutoLogin");
            }
        }

        /// <summary>
        /// 登录按钮文本
        /// </summary>
        private string _loginContent;

        public string LoginContent
        {
            get
            {
                if (_loginContent == null)
                {
                    _loginContent = LoginString;
                }
                return _loginContent;
            }
            set
            {
                _loginContent = value;
                OnPropertyChanged("LoginContent");
            }
        }

        /// <summary>
        /// 是否正在登录
        /// </summary>
        private bool _isLogin = false;

        public bool IsLogin
        {
            get { return _isLogin; }
            set
            {
                _isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }

        /// <summary>
        /// 登录进度
        /// </summary>
        private int _progressValue;

        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        /// <summary>
        /// 是否显示登录结果 
        /// </summary>
        private bool _isShowResult;

        public bool IsShowResult
        {
            get { return _isShowResult; }
            set
            {
                _isShowResult = value;
                OnPropertyChanged("IsShowResult");
            }
        }

        /// <summary>
        /// 登录结果描述
        /// </summary>
        private string _resultDescription;

        public string ResultDescription
        {
            get { return _resultDescription; }
            set
            {
                _resultDescription = value;
                OnPropertyChanged("ResultDescription");
            }
        }

        /// <summary>
        /// 登录命令
        /// </summary>
        private RelayCommand _loginCmd;

        public ICommand LoginCmd
        {
            get
            {
                if (_loginCmd == null)
                {
                    return new RelayCommand(
                        p => UserLogin());
                }

                return _loginCmd;
            }
        }

        /// <summary>
        /// 隐藏登录结果信息命令
        /// </summary>
        private RelayCommand _hideResultCmd;

        public ICommand HideResultCmd
        {
            get
            {
                if (_hideResultCmd == null)
                {
                    return new RelayCommand(
                        p => HideLoginResult(),
                        p=>IsShowResult);
                }

                return _hideResultCmd;
            }
        }
        #endregion

        #region 构造函数
        public LoginViewModel(Action closeAction)
        {
            //关闭窗口
            this._closeAction = closeAction;

            //登录线程
            _bkWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            _bkWorker.DoWork += BkDoWork;
            _bkWorker.RunWorkerCompleted += BkRunWorkerCompleted;

        }
        #endregion

        #region 命令函数
        /// <summary>
        /// 用户登录
        /// </summary>
        private void UserLogin()
        {
            IsShowResult = false;

            //取消登录
            if (LoginContent == CancelString)
            {
                if (_bkWorker.WorkerSupportsCancellation)
                {
                    IsLogin = false;
                    LoginContent = LoginString;
                    _bkWorker.CancelAsync();
                    return;
                }
            }

            //登录
            if (LoginContent == LoginString)
            {
                if (!_bkWorker.IsBusy)
                {
                    IsLogin = true;
                    _bkWorker.RunWorkerAsync();
                    LoginContent = CancelString;
                }
            }
        }

        /// <summary>
        /// 隐藏登录结果信息
        /// </summary>
        private void HideLoginResult()
        {
            ResultDescription = String.Empty;
            IsShowResult = false;
        }
        #endregion

        #region 操作函数
        /// <summary>
        /// 用户名和密码校验
        /// </summary>
        private bool VerifyUserInfo()
        {
            return true;
        }

        /// <summary>
        /// 登录线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BkDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            for (var i = 1; i <= 100; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                    ProgressValue = i + 1;
                }
            }

            if (UserName == "WELL-E" && UserPwd == "123456")
            {
                //登录成功
                e.Result = true;
                ResultDescription = String.Empty;
            }
            else
            {
                //登录失败
                e.Result = false;
                ResultDescription = "用户名或密码错误！";
            }
        }

        /// <summary>
        /// 登录完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BkRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //取消
            }
            else if (e.Error != null)
            {
                //异常
                ResultDescription = "登录异常: " + e.Error.Message;
            }
            else
            {
                if ((bool)e.Result)
                {
                    var user = new UserModel
                    {
                        UserName = UserName,
                        UserPwd = UserPwd
                    };

                    var winMain = new MainWindow();
                    Application.Current.MainWindow = winMain;
                    winMain.DataContext = new MainWindowViewModel(user);
                    _closeAction.Invoke();
                    winMain.Show();
                }
                else
                {
                    LoginContent = LoginString;
                    IsShowResult = true;
                }
            }
        }
        #endregion

    }
}
