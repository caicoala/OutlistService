using OutlistService.Application.DTOs;
using OutlistService.Domain.Entities;
using OutlistService.Domain.Interfaces;

namespace OutlistService.Application.UseCases
{
    public class OutlistUseCaseService
    {
        private readonly IOutlistRepository _repository;

        public OutlistUseCaseService(IOutlistRepository repository) => _repository = repository;

        public async Task AddAsync(OutlistProduct product)
        {
            if (product.ValidTo < product.ValidFrom)
                throw new ArgumentException("Data final não pode ser menor que a inicial.");

            var existing = await _repository.GetByProductCodeAsync(product.ProductCode);
            if (existing != null)
                throw new InvalidOperationException($"Produto com código '{product.ProductCode}' já existe.");

            await _repository.AddAsync(product);
        }

        public Task RemoveAsync(string productCode) =>
            _repository.RemoveAsync(productCode);

        public async Task UpdateValidityAsync(string code, DateTime from, DateTime to)
        {
            if (to < from)
                throw new ArgumentException("Data final não pode ser menor que a inicial.");

            var existing = await _repository.GetByProductCodeAsync(code) ?? throw new KeyNotFoundException($"Produto com código '{code}' não encontrado.");
            await _repository.UpdateValidityAsync(code, from, to);
        }

        public Task<OutlistProduct?> GetByProductCodeAsync(string code) =>
            _repository.GetByProductCodeAsync(code);

        public async Task<PagedResult<OutlistProduct>> GetPagedAsync(int page, int pageSize)
        {
            var all = await _repository.GetPagedAsync(1, int.MaxValue); // simula leitura total
            var totalItems = all.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<OutlistProduct>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

    }
}
