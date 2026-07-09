using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace chat_part2
{
    public class DatabaseService
    {
        private readonly string connectionString =
            "server=localhost;" +
            "database=ChatBot;" +
            "uid=root;" +
            "pwd=gr@yRam99;";

        public void SaveChat(string userName,
                             string userMessage,
                             string botResponse)
        {
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                        @"INSERT INTO ChatLogs
                        (UserName, UserMessage, BotResponse)
                        VALUES
                        (@user, @message, @response)";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@user", userName);
                    cmd.Parameters.AddWithValue("@message", userMessage);
                    cmd.Parameters.AddWithValue("@response", botResponse);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
           
                Console.WriteLine("DB Save Error: " + ex.Message);
            }
        }

        public void SaveTask(string userName, string task)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            INSERT INTO Tasks (UserName, TaskDescription, Status)
            VALUES (@user, @task, 'Pending')";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@user", userName);
                cmd.Parameters.AddWithValue("@task", task);

                cmd.ExecuteNonQuery();
            }
        }

      
        public List<string> GetChatHistory(string user)
        {
            List<string> history = new List<string>();

            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                        @"SELECT UserMessage, BotResponse
                          FROM ChatLogs
                          WHERE UserName=@user
                          ORDER BY Id DESC";

                    MySqlCommand cmd =
                        new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@user", user);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        history.Add("You: " + reader["UserMessage"].ToString());
                        history.Add("Bot: " + reader["BotResponse"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Read Error: " + ex.Message);
            }

            return history;
        }
    }
}