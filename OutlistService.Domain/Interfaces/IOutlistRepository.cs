using OutlistService.Domain.Entities;

namespace OutlistService.Domain.Interfaces
{
    public interface IOutlistRepository
    {
        Task AddAsync(OutlistProduct product);
        Task RemoveAsync(string productCode);
        Task UpdateValidityAsync(string productCode, DateTime from, DateTime to);
        Task<OutlistProduct?> GetByProductCodeAsync(string productCode);
        Task<List<OutlistProduct>> GetPagedAsync(int page, int pageSize);
    }
}
