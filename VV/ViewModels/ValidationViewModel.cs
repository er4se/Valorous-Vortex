using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VV.Models;
using VV.Views;

namespace VV.ViewModels
{
    public delegate void SavedUserInfoHandler();

    class ValidationViewModel : BindableBase
    {
        private readonly UserDbService userDbService;
        private User currentUser;

        private string login;    //убрать, заменить на прямое обращение к экземпляру User
        private string password; //туда же

        private bool rememberMeIsChecked = VV.Properties.Settings.Default.RememberMe;
        private event SavedUserInfoHandler savedUserInfoHandler;

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
            get { return login; }
            set 
            {
                if(SetProperty(ref login, value))
                {
                    RaisePropertyChanged(nameof(Login));
                } 
            }

        public string Password
        {
            get { return password; }
            set
            {
                if (SetProperty(ref password, value))
                {
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

                //OnLogin();                                                    //<--Дополнить функционал, возможно перенести в OnStartup
            }          

            LoginCommand = new DelegateCommand(OnLogin, CanLogin);
            RegistrationCommand = new DelegateCommand(OnRegister, CanRegister);
        }

        private bool CanLogin() => true;

        private void OnLogin()
        {
            if ((login == null) || (password == null)) 
            {
                MessageBox.Show("Введите значения в поля логин и пароль");
                return;
            }
            if (userDbService.ValidateUser(Login, Password))
            {
                userDbService.SetUserData(Login, Password);
                currentUser = userDbService.user;

                SetSavedUserInfo();

                MainViewModel mainViewModel = new MainViewModel(); //Добавить функционал переноса информации по пользователю
                mainViewModel.UserBroadcast(currentUser);

                Main main = new Main();                            //Например, новый метод, не знаю насколько это правильный подход


                main.DataContext = mainViewModel;
                main.Show();
 
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
            UserRegistration uReg = new UserRegistration();
            uReg.Show();
        }
    }
}
