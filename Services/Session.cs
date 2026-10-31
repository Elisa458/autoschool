namespace AutoSchool.Services
{
    public static class Session
    {
        public static int UserId { get; set; }
        public static string Login { get; set; } = "";
        public static string Role { get; set; } = "";
        public static bool IsAdmin => Role == "admin";
    }
}