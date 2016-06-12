namespace diexpenses.Entities
{
    using Newtonsoft.Json;

    public class Credential
    {
        private string username;
        private string password;

        public Credential(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        [JsonProperty(PropertyName = "user")]
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        [JsonProperty(PropertyName = "pass")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }


    }
}
