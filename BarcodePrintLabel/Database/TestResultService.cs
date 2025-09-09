using BarcodePrintLabel.Database.Repositories;
using BarcodePrintLabel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _repository;

        public TestResultService(ITestResultRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TestResult>> GetResultsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TestResult?> GetResultByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddResultAsync(TestResult result)
        {
            await _repository.AddAsync(result);
        }

        public async Task UpdateResultAsync(TestResult result)
        {
            await _repository.UpdateAsync(result);
        }

        public async Task DeleteResultAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
