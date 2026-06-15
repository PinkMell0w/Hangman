using System;

namespace HangmanGame.Client.Helpers
{
    public class SessionManager
    {
        private static SessionManager _instance = new SessionManager();

        private int _userId;
        private string _username;
        private string _tokenHash;

        private SessionManager() { }

        public static SessionManager Instance => _instance ?? (_instance = new SessionManager());

        public void SetSession(int userId, string username, string token)
        {
            _userId = userId;
            _username = username;
            _tokenHash = token;
        }

        public void ClearSession()
        {
            _userId = 0;
            _username = null;
            _tokenHash = null;
        }

        public bool IsSignedIn => _userId > 0 && !string.IsNullOrEmpty(_tokenHash);

        public int CurrentUserId => _userId;

        public string Username => _username;
    }
}
