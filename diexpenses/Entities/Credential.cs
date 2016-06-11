namespace diexpenses.Entities
{
    using Newtonsoft.Json;

    public class Credential
    {
        [JsonProperty(PropertyName = "user")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "pass")]
        public string Password { get; set; }

        public Credential(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
