namespace diexpenses.Entities
{
    using Newtonsoft.Json;

    public class User
    {
        private int id;
        private string authToken;
        private string username;
        private string name;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string AuthToken
        {
            get
            {
                return authToken;
            }
            set
            {
            authToken = value;
            }
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

        public override string ToString()
        {
            return base.ToString() + ": " + "Id=" + Id + ", AuthToken=" + AuthToken + ", Username=" + Username + ", Name=" + Name;
        }
    }
}
