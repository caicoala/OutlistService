using OutlistService.Domain.Entities;
using OutlistService.Domain.Interfaces;

namespace OutlistService.Application.UseCases
{
    public class OutlistUseCaseService
    {
        private readonly IOutlistRepository _repository;

        public OutlistUseCaseService(IOutlistRepository repository)
        {
            _repository = repository;
        }

        public Task AddAsync(OutlistProduct product) =>
            _repository.AddAsync(product);

        public Task RemoveAsync(string productCode) =>
            _repository.RemoveAsync(productCode);

        public Task UpdateValidityAsync(string code, DateTime from, DateTime to) =>
            _repository.UpdateValidityAsync(code, from, to);

        public Task<OutlistProduct?> GetByProductCodeAsync(string code) =>
            _repository.GetByProductCodeAsync(code);

        public Task<List<OutlistProduct>> GetPagedAsync(int page, int pageSize) =>
            _repository.GetPagedAsync(page, pageSize);
    }
}
