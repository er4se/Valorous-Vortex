using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;
using System.Data.Entity;
using System.Data.SqlClient;

namespace VV.Models
{
    public class User : BindableBase
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string NickName { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password);
    }
}
