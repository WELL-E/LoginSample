using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LoginSample.Comm;
using LoginSample.Models;

namespace LoginSample.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region 字段 属性
        /// <summary>
        /// //关闭窗口
        /// </summary>
        private readonly Action _closeAction;

        /// <summary>
        /// 
        /// </summary>
        private CancellationTokenSource _cts;

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
        /// 正在登录状态
        /// </summary>
        private bool _isLogging;

        public bool IsLogging
        {
            get { return _isLogging; }
            set
            {
                _isLogging = value;
                OnPropertyChanged("IsLogging");
                OnPropertyChanged("IsNotLogging");
            }
        }

        /// <summary>
        /// 非登录状态
        /// </summary>
        public bool IsNotLogging
        {
            get { return !_isLogging; }
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
        private ICommand _loginCmd;

        public ICommand LoginCmd
        {
            get
            {
                if (_loginCmd == null)
                {
                    return new RelayCommand(p => UserLogging());
                }

                return _loginCmd;
            }
        }

        private ICommand _cancelCmd;

        public ICommand CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    return new RelayCommand(p => _cts.Cancel());
                }

                return _cancelCmd;
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
                    return new RelayCommand(p => HideLoginResult(), p=>IsShowResult);
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

            //自动登录
            if (IsAutoLogin)
            {
                LoginCmd.Execute(null);
            }
        }
        #endregion


        #region 登录操作
        /// <summary>
        /// 用户登录
        /// </summary>
        private async void UserLogging()
        {
            if (!VerifyUserInfoLocal()) return;

            IsLogging = true;
            IsShowResult = false;

            var result = false; 
            var progressIndicator = new Progress<int>(ReportProgress);
            _cts = new CancellationTokenSource();

            try
            {
                result = await ImplLogin(progressIndicator, _cts.Token);
            }
            catch (TaskCanceledException ex) // 测试环境: Task.Delay(100, ct)
            {
                ResultDescription = ex.Message;
            }
            //catch (OperationCanceledException ex) // 生成环境: ct.ThrowIfCancellationRequested()
            //{
            //    ResultDescription = ex.Message;
            //}
            catch (Exception ex)
            {
                ResultDescription = ex.Message;
            }
            finally
            {
                _cts.Dispose();
            }

            IsLogging = false;

            //显示主窗体
            if (result)
            {
                ShowMainWindow();
            }
            else
            {
                ProgressValue = 0;
                if (!_cts.IsCancellationRequested)
                {
                    IsShowResult = true;
                }
            }
        }

        /// <summary>
        /// 执行用户登录
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<bool> ImplLogin(IProgress<int> progress, CancellationToken ct)
        {
            return await Task.Run(async () =>
            {
                var model = await GetUserInfoAsync(progress, ct);

                if (!model.UserName.Equals("WELL-E", StringComparison.OrdinalIgnoreCase)
                    || !UserPwd.Equals("123456", StringComparison.OrdinalIgnoreCase))
                {
                    ResultDescription = "用户名或密码错误！";
                    return false;
                }

                ResultDescription = String.Empty;
                return true;
            }, ct);
        }

        /// <summary>
        /// 模拟从服务端获取用户信息
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<UserModel> GetUserInfoAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await Task.Run(async () =>
            {
                for (var i = 0; i < 100; i++)
                {
                    await Task.Delay(100, ct);
                    progress.Report(i);
                }

                return new UserModel { UserName = "WELL-E", UserPwd = "123456" };
            }, ct);
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="value"></param>
        private void ReportProgress(int value)
        {
            ProgressValue = value;
        }

        /// <summary>
        /// 隐藏登录结果信息
        /// </summary>
        private void HideLoginResult()
        {
            ResultDescription = String.Empty;
            IsShowResult = false;
        }

        /// <summary>
        /// 用户名和密码校验
        /// </summary>
        private bool VerifyUserInfoLocal()
        {
            if (String.IsNullOrEmpty(UserName))
            {
                IsShowResult = true;
                ResultDescription = "用户名不能为空！";
                return false;
            }

            if (String.IsNullOrEmpty(UserPwd))
            {
                IsShowResult = true;
                ResultDescription = "密码不能为空！";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 显示主窗体
        /// </summary>
        private void ShowMainWindow()
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

        #endregion

    }
}
