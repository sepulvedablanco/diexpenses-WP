namespace diexpenses.Entities
{
    using diexpenses.Entities.Base;
    using SQLite.Net.Attributes;

    [Table("Kinds")]
    public class Kind : KindBase
    {
        public Kind() { }

        public Kind(string description)
        {
            this.Description = description;
        }
    }
}
