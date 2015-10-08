using System;
using System.ComponentModel;
using System.Windows.Input;
using LoginSample.Comm;

namespace LoginSample.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private const string StringLogin = "登录";
        private const string StringCancel = "取消";

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
                    _loginContent = StringLogin;
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
                        p => UserLogin(),
                        p => IsLogin);
                }

                return _loginCmd;
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

        public LoginViewModel(Action closeAction)
        {
            //关闭窗口
            this._closeAction = closeAction;

            //登录线程
            _bkWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _bkWorker.DoWork += BkDoWork;
            _bkWorker.ProgressChanged += BkWorkOnProgressChanged;
            _bkWorker.RunWorkerCompleted += BkRunWorkerCompleted;

        }

        /// <summary>
        /// 用户名和密码校验
        /// </summary>
        private bool VerifyUserInfo()
        {
           
            return true;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        private void UserLogin()
        {
            //取消登录
            if (LoginContent == StringCancel)
            {
                IsLogin = false;
                LoginContent = StringLogin;
                _bkWorker.CancelAsync();
            }

            //登录
            if (LoginContent == StringLogin)
            {
                if (_bkWorker.IsBusy)
                {
                    return;
                }

                IsLogin = true;
                _bkWorker.RunWorkerAsync();
                LoginContent = StringCancel;
            }
        }


        /// <summary>
        /// 隐藏登录结果信息
        /// </summary>
        private void HideLoginResult()
        {
            
        }

        /// <summary>
        /// 登录线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BkDoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 登录进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BkWorkOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 登录完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BkRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
