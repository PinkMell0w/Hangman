namespace HangmanGame.Server.Services.Helpers
{
    public class FieldValidator
    {
        public static bool IsEmptyOrWhitespace(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsValidPwd(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 8)
            {
                return false;
            }

            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumber = false;

            foreach (char c in input)
            {
                hasUpper |= char.IsUpper(c);
                hasLower |= char.IsLower(c);
                hasNumber |= char.IsDigit(c);
            }

            return hasUpper && hasLower && hasNumber;
        }

        public static bool IsValidEmail(string input)
        {
            //TODO
            return true;
        }
    }
}