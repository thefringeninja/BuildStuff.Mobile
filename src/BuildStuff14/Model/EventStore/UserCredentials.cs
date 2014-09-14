namespace BuildStuff14.Model.EventStore
{
    public class UserCredentials
    {
        public readonly string Login;
        public readonly string Password;

        public UserCredentials(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
