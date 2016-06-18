namespace diexpenses.Entities.Base
{
    using SQLite.Net.Attributes;

    public abstract class KindBase : EntityBase
    {
        private string description;

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
