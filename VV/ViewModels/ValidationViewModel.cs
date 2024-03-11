using System.Windows.Input;
using System.Windows.Data;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;

using VV.Models;
using VV.Views;
using System.Threading.Tasks;

namespace VV.ViewModels
{
    public delegate void SavedUserInfoHandler();

    class ValidationViewModel : BindableBase
    {
        private readonly UserDbService userDbService;
        private User currentUser = new User();

        private bool rememberMeIsChecked = VV.Properties.Settings.Default.RememberMe;
        private event SavedUserInfoHandler savedUserInfoHandler;

        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        public bool RememberMeIsChecked
        {
            get { return rememberMeIsChecked; }
            set
            {
                if(SetProperty(ref rememberMeIsChecked, value))
                {
                    RaisePropertyChanged(nameof(rememberMeIsChecked));
                    VV.Properties.Settings.Default.RememberMe = value;
                }
            }
        }

        public string Login
        {
            get { return currentUser.login; }
            set 
            {
                currentUser.login = value;
                RaisePropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get { return currentUser.password; }
            set
            {
                currentUser.password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
        
        public ValidationViewModel()
        {
            string connectionString = "server=DESKTOP-QOVSA9S;Trusted_Connection=Yes;DataBase=VVDB;";
            this.userDbService = new UserDbService(connectionString);

            if (rememberMeIsChecked)
            {
                Login = VV.Properties.Settings.Default.UserLogin;
                Password = VV.Properties.Settings.Default.UserPassword;

                LoginWhenChecked();
            }          

            LoginCommand = new DelegateCommand(OnLogin, CanLogin);
            RegistrationCommand = new DelegateCommand(OnRegister, CanRegister);
        }

        async private void LoginWhenChecked()                                           //Временное решение, дает время отработать show в App.xaml.cs
        {
            await Task.Delay(1000);
            OnLogin();
        }

        private bool CanLogin() => true;

        private void OnLogin()
        {
            if (isCurrentUserNull()) 
            {
                MessageBox.Show("Введите значения в поля логин и пароль");
                return;
            }
            if (userDbService.ValidateUser(Login, Password))
            {
                userDbService.SetUserData(Login, Password);                             //Лишний эксзепляр currentUser, нигде не используется по назначению
                currentUser = userDbService.user;                                       

                SetSavedUserInfo();
                CreateMainWindow(currentUser);
 
                App.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private bool CanRegister() => true;

        private void OnRegister()
        {
            UserRegistration userRegistrationInstance = new UserRegistration();
            userRegistrationInstance.Show();
        }

        private void UpdatePropertiesUserInfo()                                         //Сохраняет данные пользователя в настройки
        {
            if (!isCurrentUserNull()) 
            {
                VV.Properties.Settings.Default.UserLogin = currentUser.login;
                VV.Properties.Settings.Default.UserPassword = currentUser.password;
                VV.Properties.Settings.Default.Save();
            }
        }

        private void SetSavedUserInfo()                                                 //Выполнение событие установки сохраненных данных
        {
            if (rememberMeIsChecked)
            {
                savedUserInfoHandler += UpdatePropertiesUserInfo;
                savedUserInfoHandler();
            }
        }

        private bool isCurrentUserNull() => currentUser == null;

        private void CreateMainWindow(User currentUser)
        {
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.UserBroadcast(currentUser);

            Main main = new Main();

            main.DataContext = mainViewModel;
            main.Show();
        }
    }
}
