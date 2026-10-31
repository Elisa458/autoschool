using System;
using System.IO;
using Microsoft.Data.Sqlite;
using AutoSchool.Services;

namespace AutoSchool.Data
{
    public static class Database
    {
        public static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "autoschool.db");

        public static SqliteConnection Open()
        {
            var conn = new SqliteConnection($"Data Source={DbPath}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
            return conn;
        }

        // Создаёт таблицы и админа по умолчанию при первом запуске
        public static void Initialize()
        {
            string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "schema.sql");
            using var conn = Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = File.ReadAllText(schemaPath);
                cmd.ExecuteNonQuery();
            }

            using var check = conn.CreateCommand();
            check.CommandText = "SELECT COUNT(*) FROM users";
            if (Convert.ToInt32(check.ExecuteScalar()) == 0)
            {
                CreateUser(conn, "admin", "Admin123!", "admin");
                CreateUser(conn, "user", "User123!", "user");
            }
        }

        private static void CreateUser(SqliteConnection conn, string login, string pass, string role)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "INSERT INTO users (login, password_hash, role) VALUES ($l, $p, $r)";
            cmd.Parameters.AddWithValue("$l", login);
            cmd.Parameters.AddWithValue("$p", PasswordHasher.Hash(pass));
            cmd.Parameters.AddWithValue("$r", role);
            cmd.ExecuteNonQuery();
        }
    }
}