using RailwayBookingApp.Models;

namespace RailwayBookingApp.Services.Interfaces
{
    public interface ITrainService
    {
        Task<List<Train>> SearchTrains(string source, string destination);
        Task<Train> GetTrainById(int id);
    }
}
