using System;

namespace HangmanGame.Client.Helpers
{
    public class SessionManager
    {
        private static SessionManager _instance = new SessionManager();

        private int _userId;
        private string _tokenHash;

        private SessionManager() { }

        public static SessionManager Instance => _instance ?? (_instance = new SessionManager());

        public void SetSession(int userId, string token)
        {
            _userId = userId;
            _tokenHash = token;
        }

        public bool IsSignedIn => _userId > 0 && !string.IsNullOrEmpty(_tokenHash);

        public int CurrentUserId => _userId;
    }
}
