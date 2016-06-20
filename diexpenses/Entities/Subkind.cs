namespace diexpenses.Entities
{
    using diexpenses.Entities.Base;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;

    [Table("Subkinds")]
    public class Subkind : KindBase
    {
        private int kindId;

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
