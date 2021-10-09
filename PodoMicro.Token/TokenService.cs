using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicro.Token
{
    public class TokenService
    {
        public TokenService()
        {

        }

        public string AuthenticateUser(string username)
        {
            return TokenHandler.Instance.AuthenticateUser(username);
        }

        public bool CheckAuth(string username)
        {
            return TokenHandler.Instance.CheckIfAlreadyAuthed(username);
        }
    }
}
