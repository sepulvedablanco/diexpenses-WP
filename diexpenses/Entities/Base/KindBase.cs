namespace diexpenses.Entities.Base
{
    using SQLite.Net.Attributes;

    public abstract class KindBase
    {
        private int id;
        private int? apiId;
        private string description;

        [Column("Id"), PrimaryKey(), AutoIncrement]
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

        [Column("ApiId")]
        public int? ApiId
        {
            get
            {
                return apiId;
            }
            set
            {
                apiId = value;
            }
        }

        [Column("Description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

    }
}
