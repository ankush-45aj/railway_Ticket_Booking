namespace RailwayBookingApp.Models
{
    public class Train
    {
        public int Id { get; set; }

        public string TrainNumber { get; set; }

        public string TrainName { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public int AvailableSeats { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}
