namespace diexpenses.Entities.Base
{
    using SQLite.Net.Attributes;

    public class KindBase
    {
        [Column("Id"), PrimaryKey()]
        public int Id { get; set; }

        [Column("Description")]
        public string Description { get; set; }
    }
}
