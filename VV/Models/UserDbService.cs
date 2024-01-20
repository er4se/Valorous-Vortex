using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VV.Models
{
    public class UserDbService
    {
        private readonly string connectionString;

        public UserDbService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool ValidateUser(string login, string password)
        {
            using (SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Login = @Login AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
