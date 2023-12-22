using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;

namespace VV.Models
{
    class User : BindableBase
    {
        private int id = 0;
        private string login = string.Empty;
        private string password = string.Empty;

        public string NickName { get; set; } = string.Empty;

        public void SetUserID(int _id)
        {
            if (id == 0)
            {
                id = _id;
                RaisePropertyChanged(nameof(id));
            }
        }

        public void SetPassword(string _password)
        {
            if (password == string.Empty)
            {
                password = _password;
                RaisePropertyChanged(nameof(password));
            }
        }

        public void SetLogin(string _login)
        {
            if (login == string.Empty)
            {
                login = _login;
                RaisePropertyChanged(nameof(login));
            }
        }

        public void SetNickname(string _nickname)
        {
            if(NickName != string.Empty)
            {

            }

            NickName = _nickname;
            RaisePropertyChanged(nameof(NickName));
        }
    }
}
