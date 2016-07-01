namespace common.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class MovementsPage
    {
        [JsonProperty("Page")]
        private IList<Movement> movements;

        private int totalMovements;

        public IList<Movement> Movements
        {
            get
            {
                return movements;
            }
            set
            {
                movements = value;
            }
        }

        public int TotalMovements
        {
            get
            {
                return totalMovements;
            }
            set
            {
                totalMovements = value;
            }
        }

    }
}
