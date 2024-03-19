namespace TODOBack.Models
{
    public class PasswordHash
    {
        public static string Encrypt(string password)
        {
            int workFactor = 15;
            
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        public static bool Decrypt(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
