using Prism.Commands;
using Prism.Mvvm;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VV.Views;
using System.Windows.Data;
using System.Windows.Controls;

namespace VV.ViewModels
{
    class SignIn_VM : BindableBase
    {
        public string userLogin { get; set; } = string.Empty;
        public string userPassword { get; set; } = string.Empty;

        public SignIn_VM()
        {

        }

        public DataTable Select(string selectSQL) // функция подключения к базе данных и обработка запросов
        {
            DataTable dataTable = new DataTable("dataBase");                // создаём таблицу в приложении
                                                                            // подключаемся к базе данных
            SqlConnection sqlConnection = new SqlConnection("server=DESKTOP-QOVSA9S;Trusted_Connection=Yes;DataBase=VVDB;");
            sqlConnection.Open();                                           // открываем базу данных
            SqlCommand sqlCommand = sqlConnection.CreateCommand();          // создаём команду
            sqlCommand.CommandText = selectSQL;                             // присваиваем команде текст
            sqlCommand.Parameters.AddWithValue("@userLogin", userLogin);
            sqlCommand.Parameters.AddWithValue("@userPassword", userPassword);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
            sqlDataAdapter.Fill(dataTable);                                 // возращаем таблицу с результатом
            return dataTable;
        }


        public ICommand SignInTry
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if((userLogin != string.Empty) && (userPassword != string.Empty)) 
                    {
                        DataTable dt_user = Select("SELECT ID, Login, Password, Nickname FROM [dbo].[users]" +
                            "WHERE Login = @userLogin AND Password = @userPassword");

                        if (dt_user.Rows.Count == 0) MessageBox.Show("Неверные данные пользователя");
                        else
                        {
                            Main mainPage = new Main();
                            mainPage.Show();
                        }
                    }
                }
                );
            }
        }
    }
}
