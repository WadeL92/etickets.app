using System;
using System.Security.Cryptography;
using System.Text;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        Console.WriteLine("Création de compte");

        Console.Write("Nom user ");
        string username = Console.ReadLine();

        Console.Write("MDP ");
        string password = Console.ReadLine();

        string uniqueKey = GenerateUniqueKey();

        Console.WriteLine($"Nom user: {username}");
        Console.WriteLine($"MDP: {password}");
    }

    static string GenerateUniqueKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[28];
            rng.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "").ToUpper();
        }
    }

    static void CreatDatabaseAndTable(string eticketDatabase)
    {
        using (var dbConnect = new SQLiteConnection(eticketDatabase))
        {
            dbConnect.Open();
            string creatTableQuery = @" CREATE TABLE Accounts ( Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Username TEXT NOT NULL
                                      Password TEXT NOT NULL
                                      UniqueKey TEXT NOT NULL )";

        using (var dbQuery = new SQLiteCommand(creatTableQuery))
            {
                dbQuery.ExecuteNonQuery();
            }
        }
    }

    static void AccountData(string eticketDatabase, string username, string password, string uniqueKey)
    {
        using (var dbConnect =  new SQLiteConnection(eticketDatabase))
        {
            dbConnect.Open();
            string insertQuery = "INSERT INTO Accounts (Username, Password, UniqueKey) VALUES (@Username, @Password, @UniqueKey)";
            using (var dbQuery = new SQLiteCommand(insertQuery))
            {
                dbQuery.Parameters.AddWithValue("@Username", username);
                dbQuery.Parameters.AddWithValue("@Password", password);
                dbQuery.Parameters.AddWithValue("@UniqueKey", uniqueKey);
                dbQuery.ExecuteNonQuery();
            }
        }
    }
}