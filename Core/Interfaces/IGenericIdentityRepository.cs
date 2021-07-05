using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Identity;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericIdentityRepository<T> where T : AppUser
    {
         Task<T> GetByIdAsync(int id);
         Task<IReadOnlyList<T>> ListAllAsync();
         Task<T> GetEntityWithSpec(ISpecification<T> spec);
         Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
         Task<int> CountAsync(ISpecification<T> spec);
         void Add(T entity);
         void Update(T entity);
         void Delete(T entity);
    }
}