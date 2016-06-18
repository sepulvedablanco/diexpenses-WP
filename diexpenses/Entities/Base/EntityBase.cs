namespace diexpenses.Entities.Base
{
    using SQLite.Net.Attributes;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class EntityBase : INotifyPropertyChanged
    {
        private int? id;
        private int? apiId;

        [Column("Id"), PrimaryKey(), AutoIncrement]
        public int? Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
