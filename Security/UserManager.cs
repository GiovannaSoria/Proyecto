using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    public class UserManager
    {
        public Users Authenticate(string username, string password)
        {
            using (var context = new Sales_DBEntities())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user == null || !PasswordManager.VerifyPassword(password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
                }

                return user;
            }
        }


        public void Register(string username, string password, string email, string role)
        {
            using (var context = new Sales_DBEntities())
            {
                var newUser = new Users
                {
                    Username = username,
                    PasswordHash = PasswordManager.HashPassword(password),
                    Email = email,
                    Role = role
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }
    }
}
