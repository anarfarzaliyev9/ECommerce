using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce_API.Abstractions
{
    public interface IGeneralRepo<T>
    {
        Task<List<T>> GetAll();

        Task<T> GetById(string id);
        Task<T> Create(T entity);
        Task<bool> Edit(T entity);
        Task<bool> Delete(string id);
    }
}
