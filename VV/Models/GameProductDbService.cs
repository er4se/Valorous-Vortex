using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VV.Models
{
    public class GameProductDbService
    {
        private readonly string connectionString;
        private GameProduct gameProduct = new GameProduct();

        public GameProductDbService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public GameProduct setGameProduct()
        {
            return gameProduct;
        }

        public void SelectGameProduct(string productName)
        {
            string sqlExpression = "SELECT ID, GameName, DeveloperName FROM [dbo].[GameProducts] WHERE GameName = @CurrentGameName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@CurrentGameName", productName);

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object id = reader.GetValue(0);
                        object gameName = reader.GetValue(1);
                        object developerName = reader.GetValue(2);

                        gameProduct.GameProductId = id.ToString();
                        gameProduct.GameProductName = gameName.ToString();
                        gameProduct.GameProductDeveloper = developerName.ToString();

                        UploadImageGameProduct(productName);
                    }
                }

                reader.Close();
            }
        }

        public void UploadImageGameProduct(string productName)
        {
            string sqlExpression = "SELECT SplashScreen FROM [dbo].[GameProducts] WHERE GameName = @CurrentGameName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.Parameters.AddWithValue("@CurrentGameName", productName);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    if(reader.Read())
                    {
                        byte[] imageData = (byte[])reader.GetValue(0);
                        gameProduct.GameProductSplashScreen = imageData;
                    }
                }

                reader.Close();
            }
        }
    }
}
