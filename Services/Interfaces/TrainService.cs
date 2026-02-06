using Microsoft.EntityFrameworkCore;
using RailwayBookingApp.Data;
using RailwayBookingApp.Models;
using RailwayBookingApp.Services.Interfaces;

namespace RailwayBookingApp.Services
{
    public class TrainService : ITrainService
    {
        private readonly AppDbContext _context;

        public TrainService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Train>> SearchTrains(string source, string destination)
        {
            return await _context.Trains
                .Where(t => t.Source == source && t.Destination == destination)
                .ToListAsync();
        }

        public async Task<Train> GetTrainById(int id)
        {
            return await _context.Trains.FindAsync(id);
        }
    }
}
