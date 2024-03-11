using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VV.Models;

namespace VV.ViewModels
{
    class RegistrationViewModel : BindableBase                                      //ViewModel механики регистрации пользователя
    {
        private readonly UserDbService userDbService;                               //Для сопоставления данных используется прямое обращение к БД
        private User newCreatedUser = new User();
        public Action CloseAction { get; set; }
        public ICommand RegistrationCommand { get; }

        public string NewLogin
        {
            get { return newCreatedUser.login; }
            set
            {
                newCreatedUser.login = value;
                RaisePropertyChanged(nameof(NewLogin));
            }
        }

        public string NewNickname
        {
            get { return newCreatedUser.NickName; }
            set
            {
                newCreatedUser.NickName = value;
                RaisePropertyChanged(nameof(NewNickname));              
            }
        }

        public string NewPassword
        {
            get { return newCreatedUser.password; }
            set
            {
                newCreatedUser.password = value;
                RaisePropertyChanged(nameof(NewPassword));
            }
        }


        public RegistrationViewModel()
        {
            string connectionString = "server=DESKTOP-QOVSA9S;Trusted_Connection=Yes;DataBase=VVDB;";
            this.userDbService = new UserDbService(connectionString);

            RegistrationCommand = new DelegateCommand(OnRegister, CanRegister);
        }

        private bool CanRegister() => true;

        async private void OnRegister()                                             //При инициализации регистрации инициализируется процедура проверки
        {                                                                           //созданная предварительно в качестве метода класса UserDbService
            if (userDbService.isUniqueUser(newCreatedUser.login))                   //При успешной уникальности логина пользователя
            {
                await Task.Run(() =>
                {                                                                   //генерируется ID нового пользователя, полные данные экземпляра
                    newCreatedUser.id = GenerateUserID();                           //нового пользователя отправляются в базу данных по средствам
                    userDbService.InsertUser(newCreatedUser);                       //специально созданного метода InsertUser в классе UserDbService
                });
                MessageBox.Show("Успешная регистрация");
                CloseAction();
            }
            else
            {
                MessageBox.Show("Логин уже занят");
            }
        }

        private string GenerateUserID() => Guid.NewGuid().ToString("N");            //Генератор ID пользователя
    }
}
