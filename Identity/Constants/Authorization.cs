namespace Identity.Constants
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            User
        }
        public const string default_username = "user";
        public const string default_email = "user@identityapi.com";
        public const string default_password = "password";
        public const Roles default_role = Roles.User;
    }
}
