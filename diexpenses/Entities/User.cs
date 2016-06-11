namespace diexpenses.Entities
{
    using Newtonsoft.Json;

    public class User
    {
        public int Id { get; set; }
        public string AuthToken { get; set; }
        [JsonProperty(PropertyName = "user")]
        public string Username { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return base.ToString() + ": " + "Id=" + Id + ", AuthToken=" + AuthToken + ", Username=" + Username + ", Name=" + Name;
        }
    }
}
