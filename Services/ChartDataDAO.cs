using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using ThinkfulApp.Models;

namespace ThinkfulApp.Services
{
    public class ChartDataDAO
    {
        private static string connectionString = "Data Source=thinkful-azure-server.database.windows.net;Initial Catalog=ThinkFulDB;User ID=CloudSAe0a7b040;Password=Manuel12;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void setConnectionString(string str)
        {
            connectionString = str;
        }

        // WORKING
        public static ChartModel? GetChartFromId(int id)
        {
            ChartModel? foundChart = null;
            string commandString = "SELECT * FROM dbo.Charts WHERE Id = @Id\n";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        foundChart = new ChartModel
                        {
                            Id = (int)reader[0],
                            NumOfGoals = (int)reader[1]
                        };
                        var JSONInfo = JObject.Parse(reader.GetString(2));
                        List<string> Keys = JSONInfo.Properties().Select(p => p.Name).ToList();
                        foreach (string Key in Keys)
                        {
                            foundChart.GoalList.Add((int)JSONInfo[Key]);
                            foundChart.LabelList.Add(Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return foundChart;
        }

        // WORKING
        public static int Update(ChartModel chart)
        {
            int id = -1;
            if (chart.NumOfGoals < 1)
            {
                return id;
            }

            string commandString = @"UPDATE dbo.Charts SET Goals = JSON_QUERY('{""' + CONVERT(VARCHAR(MAX), @Label1) + '"": ' + CONVERT(varchar(MAX), @Goal1) + '";
            string commandSuffix = @"}') WHERE Id = @Id";
            for (int i = 1; i < chart.NumOfGoals; i++)
            {
                string aux = @$", ""' + CONVERT(VARCHAR(MAX), @Label{i + 1}) + '"": ' + CONVERT(VARCHAR(MAX), @Goal{i + 1}) + '";
                commandString += aux;
            }
            commandString += commandSuffix;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@NumOfGoals", chart.NumOfGoals);
                command.Parameters.AddWithValue("@Id", chart.Id);
                for (int i = 0; i < chart.NumOfGoals; i++)
                {
                    command.Parameters.AddWithValue($"@Label{i + 1}", chart.LabelList[i]);
                    command.Parameters.AddWithValue($"@Goal{i + 1}", chart.GoalList[i]);
                }

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    id = chart.Id;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return id;
        }

        // WORKING
        public static string DeleteGoal(int Id, string DeletedGoal)
        {
            string deleted = "Failure";

            string commandString1 = "SELECT NumOfGoals FROM dbo.Charts WHERE Id = @Id\n";
            string commandString2 = @"UPDATE dbo.Charts SET Goals = JSON_MODIFY(Goals, '$.""'+@DeletedGoal+'""', NULL), NumOfGoals = NumOfGoals - 1 WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandString1, connection);

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@DeletedGoal", DeletedGoal);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int NumOfGoals = reader.GetInt32(0);
                        reader.Close();
                        if (NumOfGoals > 3)
                        {
                            command.CommandText = commandString2;
                            command.ExecuteNonQuery();
                            deleted = "Success";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return deleted;
        }

        // TODO EVERYTHING
        public static string AddGoal(int id, string newLabel, int newValue)
        {
            string goalMade = null;

            string commandString = @"UPDATE dbo.Charts SET Goals = JSON_MODIFY(Goals, '$.""'+@NewLabel+'""', @NewValue), NumOfGoals = NumOfGoals + 1 WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@NewLabel", newLabel);
                command.Parameters.AddWithValue("@NewValue", newValue);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    goalMade = newLabel;
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }
            return goalMade;
        }
    }
}
