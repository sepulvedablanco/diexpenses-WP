namespace common.Entities
{
    using common.Entities.Base;
    using SQLite.Net.Attributes;

    [Table("Kinds")]
    public class Kind : KindBase
    {
        public Kind() { }

        public Kind(int id)
        {
            this.Id = id;
        }

        public Kind(string description)
        {
            this.Description = description;
        }
    }
}
