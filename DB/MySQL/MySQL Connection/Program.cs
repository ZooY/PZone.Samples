using System;
using MySql.Data.MySqlClient;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            var connectionString = "Database=БАЗА;Data Source=СЕРВЕР;User Id=ПОЛЬЗОВАТЕЛЬ;Password=ПАРОЛЬ";
            // БАЗА - имя базы данных (схемы) в MySQL
            // СЕРВЕР - имя или IP-адрес сервера (для локального можно использовать localhost)
            // ПОЛЬЗОВАТЕЛЬ - имя пользователя MySQL
            // ПАРОЛЬ - пароль пользователя MySQL
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();
                var commandText = "SELECT * FROM accounts;";
                using (var cmd = new MySqlCommand(commandText, con))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var name = rdr.GetString("name");
                        }
                    }
                }
            }


            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}