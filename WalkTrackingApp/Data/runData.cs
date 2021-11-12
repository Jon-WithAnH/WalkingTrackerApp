using SQLite;

namespace WalkTrackingApp.Data
{
    public class runData
    {
            
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string time { get; set; }
        public double distance { get; set; }
        // distance rounds to second decminal
        public double mph { get; set; }
        // mph rounds to third decminal
        public string dayTime { get; set; }

        public string allLocationPoints { get; set; }




    }
}
