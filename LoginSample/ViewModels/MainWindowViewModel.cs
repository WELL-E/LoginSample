
using LoginSample.Models;

namespace LoginSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private UserModel _user;

        public MainWindowViewModel(UserModel user)
        {
            _user = user;
        }

        public string UserName
        {
            get { return _user.UserName; }
        }
    }
}
