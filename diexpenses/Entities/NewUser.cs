namespace diexpenses.Entities
{
    public class NewUser: Credential
    {
        public string Name { get; set; }

        public NewUser(string username, string password) : base(username, password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
