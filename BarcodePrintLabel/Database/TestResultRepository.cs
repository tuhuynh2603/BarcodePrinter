using BarcodePrintLabel.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Database.Repositories
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly DatabaseContext _context;

        public TestResultRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestResult>> GetAllAsync()
        {
            return await _context.TestResults.AsNoTracking().ToListAsync();
        }

        public async Task<TestResult?> GetByIdAsync(int id)
        {
            return await _context.TestResults.FindAsync(id);
        }

        public async Task AddAsync(TestResult result)
        {
            await _context.TestResults.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TestResult result)
        {
            _context.TestResults.Update(result);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TestResults.FindAsync(id);
            if (entity != null)
            {
                _context.TestResults.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
