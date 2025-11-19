using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByCpfAsync(string cpf);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
