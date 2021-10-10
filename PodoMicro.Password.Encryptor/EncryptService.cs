using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicro.Password.Encryptor
{
    public class EncryptService
    {
        public bool CheckPassword(string password, byte[] passwordSalt,byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            } 
        }

        public PasswordDTO EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return null;
            using (var hmac = new HMACSHA512())
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var passwordSalt = hmac.Key;
                return new PasswordDTO()
                {
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
            }
                
        }
    }
}
