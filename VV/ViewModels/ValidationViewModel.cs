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

using System.Windows;

namespace VV.ViewModels
{
    class ValidationViewModel : BindableBase
    {
        private readonly UserDbService userDbService;

        private string login;
        private string password;

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
        }

        public ICommand LoginCommand { get; }
        
        public ValidationViewModel()
        {
            string connectionString = "server=DESKTOP-QOVSA9S;Trusted_Connection=Yes;DataBase=VVDB;";
            this.userDbService = new UserDbService(connectionString);            

            LoginCommand = new DelegateCommand(OnLogin, CanLogin);
        }
        
        private bool CanLogin()
        {
            return true;
        }
            //Добавить логику от бд

        private void OnLogin()
        {
            if (userDbService.ValidateUser(Login, Password))
            {
                MainViewModel mainViewModel = new MainViewModel();
                Main main = new Main();

                main.DataContext = mainViewModel;
                main.Show();

                App.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
