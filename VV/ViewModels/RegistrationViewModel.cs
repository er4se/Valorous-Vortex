using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VV.Models;
using VV.Views;

namespace VV.ViewModels
{
    internal class RegistrationViewModel : BindableBase
    {
        private readonly UserDbService userDbService;
        private User newUser = new User();

        private string newLogin;
        private string newNickname;
        private string newPassword;

        public string NewLogin
        {
            get { return newLogin; }
            set
            {
                if(SetProperty(ref newLogin, value))
                {
                    RaisePropertyChanged(nameof(newLogin));
                }
            }
        }

        public string NewNickname
        {
            get { return newNickname; }
            set
            {
                if (SetProperty(ref newNickname, value))
                {
                    RaisePropertyChanged(nameof(newNickname));
                }
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                if (SetProperty(ref newPassword, value))
                {
                    RaisePropertyChanged(nameof(newPassword));
                }
            }
        }

        public ICommand RegistrationCommand { get; }

        public RegistrationViewModel()
        {
            string connectionString = "server=DESKTOP-QOVSA9S;Trusted_Connection=Yes;DataBase=VVDB;";
            this.userDbService = new UserDbService(connectionString);

            RegistrationCommand = new DelegateCommand(OnRegister, CanRegister);
        }

        private bool CanRegister() => true;

        private void OnRegister()
        {
            if(userDbService.isUniqueUser(newLogin))
            {
                newUser.login = newLogin;
                newUser.password = newPassword;
                newUser.NickName = newNickname;
                newUser.id = Guid.NewGuid().ToString("N");

                userDbService.InsertUser(newUser);

                MessageBox.Show("Успешная регистрация");
            }
            else
            {
                MessageBox.Show("Логин уже занят");
            }
        }

    }
}
