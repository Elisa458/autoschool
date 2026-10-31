using Microsoft.Data.Sqlite;
using AutoSchool.Data;

namespace AutoSchool.Services
{
    public static class AuthService
    {
        public static bool Login(string login, string password)
        {
            using var conn = Database.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT id, password_hash, role FROM users WHERE login = $l";
            cmd.Parameters.AddWithValue("$l", login);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return false;

            if (!PasswordHasher.Verify(password, r.GetString(1))) return false;

            Session.UserId = r.GetInt32(0);
            Session.Login = login;
            Session.Role = r.GetString(2);
            return true;
        }
    }
}