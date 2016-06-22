namespace diexpenses.Entities
{
    using diexpenses.Entities.Base;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;

    [Table("Subkinds")]
    public class Subkind : KindBase
    {
        private int kindId;

        public Subkind() { }

        public Subkind(int id)
        {
            this.Id = id;
        }

        public Subkind(int kindId, string description)
        {
            this.KindId = kindId;
            this.Description = description;
        }

        [ForeignKey(typeof(Kind))]
        public int KindId
        {
            get
            {
                return kindId;
            }
            set
            {
                kindId = value;
                RaisePropertyChanged();
            }
        }
    }
}
