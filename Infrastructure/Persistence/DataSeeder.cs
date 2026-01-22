using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]

    public class DataSeeder
    {

        private readonly AppDbContext _context;
        private bool _seedInDb = false;
        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task Initialize()
        {
            await SeedCustomer();

            if (_seedInDb)
                await _context.SaveChangesAsync();
        }


        private async Task SeedCustomer()
        {
            var customerDb = await _context.Customer.FirstOrDefaultAsync();

            if (customerDb is null)
            {
                 await _context.Customer.AddAsync(new CustomerDbModel(Guid.Parse("00000000-0000-0000-0000-000000000001"), "02940123039", "Alexandre Alencar", "ale.alencarr@outlook.com.br", true, DateTime.Now));
                _seedInDb = true;
            }
        }

    }
}
