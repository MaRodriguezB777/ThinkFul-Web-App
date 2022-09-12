using System.Data.SqlClient;
using ThinkfulApp.Models;

namespace ThinkfulApp.Services
{
    public class UsersDAO
    {
        static readonly String connectionStringOriginal = @"Data Source=thinkful-azure-server.database.windows.net;Initial Catalog=ThinkFulDB;User ID=CloudSAe0a7b040;Password=Manuel12;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        static public UserModel GetUserById(int id)
        {
            UserModel foundUser = null;
            string commandString = "SELECT * from dbo.UserInfo WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionStringOriginal))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        foundUser = new UserModel
                        {
                            Id = (int)reader[0],
                            Username = (string)reader[1],
                            Password = (string)reader[2],
                            Name = (string)reader[3],
                            Interest = (string)reader[4]
                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return foundUser;
            }
        }
        static public UserModel VerifyAndGetUser(UserModel user)
        {
            UserModel foundUser = null;

            string commandString = "SELECT * FROM dbo.UserInfo WHERE Username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionStringOriginal))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        foundUser = new UserModel
                        {
                            Id = (int)reader[0],
                            Username = (string)reader[1],
                            Password = (string)reader[2],
                            Name = (string)reader[3],
                            Interest = (string)reader[4]
                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return foundUser;
            }
        }

        static public int Insert(UserModel user)
        {
            int newIdNumber = -1;

            string commandString =
                "BEGIN TRY\n" +
                "INSERT INTO dbo.UserInfo VALUES (@Username, @Password, @Name, @Interest)\n" +
                "INSERT INTO dbo.Charts DEFAULT VALUES\n" +
                "SELECT MAX(Id) FROM dbo.UserInfo\n" +
                "END TRY\n" +
                "BEGIN CATCH\n" +
                "RETURN\n" +
                "END CATCH\n";

            using (SqlConnection connection = new SqlConnection(connectionStringOriginal))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Interest", user.Interest);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        newIdNumber = (int)reader[0];
                    }
                }
                catch (Exception ex)
                {
                    if (ex is not SqlException)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return newIdNumber;
        }
        static public int GetNextId()
        {
            int nextId = -1;
            string commandString = "SELECT MAX(Id) FROM dbo.UserInfo";
            using (SqlConnection connection = new SqlConnection(connectionStringOriginal))
            {
                SqlCommand command = new SqlCommand(commandString, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        nextId = (int)reader[0] + 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return nextId; //Returns the next Id that would be inserted into the Db.

        }
    }
}
