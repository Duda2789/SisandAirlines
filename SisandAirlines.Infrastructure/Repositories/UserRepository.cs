using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces;

namespace SisandAirlines.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            const string sql = @"
                SELECT 
                    id AS Id,
                    full_name AS FullName,
                    email AS Email,
                    cpf AS Cpf,
                    birth_date AS BirthDate,
                    password_hash AS PasswordHash,
                    created_at AS CreatedAt
                FROM users
                WHERE id = @Id;
            ";

            return await _unitOfWork.Connection
                .QueryFirstOrDefaultAsync<User>(sql, new { Id = id }, _unitOfWork.Transaction);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = @"
                SELECT 
                    id AS Id,
                    full_name AS FullName,
                    email AS Email,
                    cpf AS Cpf,
                    birth_date AS BirthDate,
                    password_hash AS PasswordHash,
                    created_at AS CreatedAt
                FROM users
                WHERE email = @Email;
            ";

            return await _unitOfWork.Connection
                .QueryFirstOrDefaultAsync<User>(sql, new { Email = email }, _unitOfWork.Transaction);
        }

        public async Task<User?> GetByCpfAsync(string cpf)
        {
            const string sql = @"
                SELECT 
                    id AS Id,
                    full_name AS FullName,
                    email AS Email,
                    cpf AS Cpf,
                    birth_date AS BirthDate,
                    password_hash AS PasswordHash,
                    created_at AS CreatedAt
                FROM users
                WHERE cpf = @Cpf;
            ";

            return await _unitOfWork.Connection
                .QueryFirstOrDefaultAsync<User>(sql, new { Cpf = cpf }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            const string sql = @"
                SELECT 
                    id AS Id,
                    full_name AS FullName,
                    email AS Email,
                    cpf AS Cpf,
                    birth_date AS BirthDate,
                    password_hash AS PasswordHash,
                    created_at AS CreatedAt
                FROM users;
            ";

            return await _unitOfWork.Connection
                .QueryAsync<User>(sql, transaction: _unitOfWork.Transaction);
        }

        public async Task AddAsync(User user)
        {
            const string sql = @"
                INSERT INTO users
                    (id, full_name, email, cpf, birth_date, password_hash, created_at)
                VALUES
                    (@Id, @FullName, @Email, @Cpf, @BirthDate, @PasswordHash, @CreatedAt);
            ";

            await _unitOfWork.Connection.ExecuteAsync(sql, new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Cpf,
                user.BirthDate,
                user.PasswordHash,
                user.CreatedAt
            }, _unitOfWork.Transaction);
        }

        public async Task UpdateAsync(User user)
        {
            const string sql = @"
                UPDATE users
                SET
                    full_name = @FullName,
                    email      = @Email,
                    cpf        = @Cpf,
                    birth_date = @BirthDate,
                    password_hash = @PasswordHash
                WHERE id = @Id;
            ";

            await _unitOfWork.Connection.ExecuteAsync(sql, new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Cpf,
                user.BirthDate,
                user.PasswordHash
            }, _unitOfWork.Transaction);
        }

        public async Task DeleteAsync(Guid id)
        {
            const string sql = @"DELETE FROM users WHERE id = @Id;";

            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }
    }
}
