using BarcodePrintLabel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Services
{
    public interface ITestResultService
    {
        Task<IEnumerable<TestResult>> GetResultsAsync();
        Task<TestResult?> GetResultByIdAsync(int id);
        Task AddResultAsync(TestResult result);
        Task UpdateResultAsync(TestResult result);
        Task DeleteResultAsync(int id);
    }
}
