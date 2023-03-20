﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartService.Data
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task RemoveAsync(T entity);
        ApplicationDbContext GetContext();
        Task SaveChangesAsync();
    }
}
