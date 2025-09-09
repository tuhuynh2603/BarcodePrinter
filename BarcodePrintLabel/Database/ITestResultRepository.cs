using BarcodePrintLabel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Database.Repositories
{
    public interface ITestResultRepository
    {
        Task<IEnumerable<TestResult>> GetAllAsync();
        Task<TestResult?> GetByIdAsync(int id);
        Task AddAsync(TestResult result);
        Task UpdateAsync(TestResult result);
        Task DeleteAsync(int id);
    }
}
