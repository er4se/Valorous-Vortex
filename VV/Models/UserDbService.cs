﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace VV.Models
{
    public class UserDbService
    {
        private readonly string connectionString;
        public User user = new User();

        public UserDbService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void SetUserData(string login, string password)
        {
            string sqlExpression = "SELECT ID, Nickname FROM [dbo].[Users] WHERE Login = @Login AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object id = reader.GetValue(0);
                        object nickname = reader.GetValue(1);

                        user.id = id.ToString();          //Внос данных в User
                        user.NickName = nickname.ToString();    //В будущем будет рассматриваться изменение
                        user.login = login;                     //алгоритма в сторону большей оптимизации и безопасности
                        user.password = password;
                    }
                }

                reader.Close();
            }
        }

        public bool ValidateUser(string login, string password)
        {
            using (SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Users] WHERE Login = @Login AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();

                    //if(count > 0) SetUserData(login, password); //Если находит такого пользователя, то его данные считываются

                    return count > 0;
                }
            }
        }

        public bool isUniqueUser(string login)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Users] WHERE Login = @Login", connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    int count = (int)command.ExecuteScalar();

                    return count == 0;
                }
            }
        }

        public void InsertUser(User newUser)
        {
            string sqlExpression = "INSERT INTO [dbo].[Users] ([ID],[Login],[Password],[Nickname]) VALUES (@ID, @Login, @Password, @Nickname)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@ID", newUser.id);
                command.Parameters.AddWithValue("@Login", newUser.login);
                command.Parameters.AddWithValue("@Password", newUser.password);
                command.Parameters.AddWithValue("@Nickname", newUser.NickName);

                    command.ExecuteNonQuery();
            }
        }
    }
}
