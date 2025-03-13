namespace Pilotic.Core.Interfaces;

public interface IRepository<T> where T : IRepositoryEntity
{
    Task<T?> GetById(string id, CancellationToken cancellationToken = default);
    Task<T> Update(T issue, CancellationToken cancellationToken = default);
    Task<T> Add(T issue, CancellationToken cancellationToken = default);
    Task Delete(string id, CancellationToken cancellationToken = default);
}
