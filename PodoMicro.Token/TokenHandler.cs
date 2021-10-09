using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PodoMicro.Token
{
    class TokenHandler
    {
        private static TokenHandler instance = null;
        private static readonly object padlock = new object();
        private ConcurrentDictionary<string, string> _Tokens;
        private ConcurrentDictionary<string, DateTime> _TokensDate;
        private Timer _Timer;

        TokenHandler()
        {
            _Tokens = new ConcurrentDictionary<string, string>();
            _TokensDate = new ConcurrentDictionary<string, DateTime>();
            _Timer = new Timer(TimeSpan.FromMinutes(20).TotalMilliseconds);
            // Hook up the Elapsed event for the timer. 
            _Timer.Elapsed += OnTimedEvent;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

            _Timer.Enabled = true;
            try
            {
                List<string> username = new List<string>();
                List<string> token = new List<string>();
                foreach (var item in _TokensDate)
                {
                    if (item.Value.AddMinutes(20) > DateTime.Now)
                    {
                        var user = _Tokens.SingleOrDefault(s => s.Value == item.Key);
                        if (!user.Equals(default(KeyValuePair<string, string>)))
                        {
                            username.Add(user.Key);
                            token.Add(user.Value);
                        }
                    }
                }
                foreach (var item in username)
                {
                    string tok;
                    _Tokens.TryRemove(item, out tok);
                }
                foreach (var item in token)
                {
                    DateTime tok;
                    _TokensDate.TryRemove(item, out tok);
                }
            }
            catch
            {

            } 
            _Timer.Enabled = false;
        }
   

        internal static TokenHandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TokenHandler();
                    }
                    return instance;
                }
            }
        }

        private string CreateToken()
        {
            lock (padlock)
            {
                Guid token = Guid.NewGuid();
            var plainTextBytes = Encoding.UTF8.GetBytes(token.ToString());
            return Convert.ToBase64String(plainTextBytes);
            }
        }

        internal bool CheckIfAlreadyAuthed(string username)
        {
            lock (padlock)
            {
                return _Tokens.ContainsKey(username);
            }
        }

        internal string AuthenticateUser(string username)
        {
            lock (padlock)
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    if (CheckIfAlreadyAuthed(username))
                    {
                        string tok;
                        var dt= DateTime.Now;
                        if (!_Tokens.TryRemove(username, out tok)) return null;
                        if (!_TokensDate.TryRemove(tok, out dt)) return null;
                    }
                    var token = CreateToken();
                    _Tokens.TryAdd(username, token);
                    _TokensDate.TryAdd(token, DateTime.Now);
                    return token;
                }
                catch
                {
                    return null;
                }
                
            }
        }
    }
}
