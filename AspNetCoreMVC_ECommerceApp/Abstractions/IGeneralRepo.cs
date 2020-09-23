using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce_API.Abstractions
{
    public interface IGeneralRepo<T>
    {
        Task<List<T>> GetAll();

        Task<T> GetById(int id);
        Task<T> Create(T entity);
        Task<bool> Edit(T entity);
        Task<bool> Delete(int id);
    }
}
