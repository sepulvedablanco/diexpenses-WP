namespace common.Entities
{
    using Newtonsoft.Json;

    public class NewUser : Credential
    {
        private string name;

        public NewUser(string username, string password) : base(username, password)
        {
            this.Username = username;
            this.Password = password;
        }

        public NewUser(string name, string username, string password) : this(username, password)
        {
            this.Name = name;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}