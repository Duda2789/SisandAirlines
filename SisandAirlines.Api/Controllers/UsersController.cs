using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Api.Models;
using SisandAirlines.Domain.Interfaces;
using SisandAirlines.Domain.Entities;

namespace SisandAirlines.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: users/getall
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            var result = users.Select(u => new UserResponse
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Cpf = u.Cpf,
                BirthDate = u.BirthDate
            });

            return Ok(result);
        }

        // GET: users/getbyid
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                return NotFound();

            var dto = new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Cpf = user.Cpf,
                BirthDate = user.BirthDate
            };

            return Ok(dto);
        }

        // POST: users/create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.BeginAsync();

            try
            {
                // TODO: aqui o ideal é fazer hash da senha (BCrypt, por exemplo)
                var user = new User(
                    request.FullName,
                    request.Email,
                    request.Cpf,
                    request.BirthDate,
                    passwordHash: request.Password // por enquanto direto, pra simplificar
                );

                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();

                var dto = new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Cpf = user.Cpf,
                    BirthDate = user.BirthDate
                };

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, dto);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        // PUT: users/update
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.BeginAsync();

            try
            {
                var existing = await _userRepository.GetByIdAsync(id);
                if (existing is null)
                    return NotFound();

                // aqui usamos o método Update da entidade de domínio
                existing.Update(
                    request.FullName,
                    request.Email,
                    request.Cpf,
                    request.BirthDate,
                    request.Password // pode ser null, a entidade trata
                );

                await _userRepository.UpdateAsync(existing);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }


        // DELETE: api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _unitOfWork.BeginAsync();

            try
            {
                var existing = await _userRepository.GetByIdAsync(id);
                if (existing is null)
                    return NotFound();

                await _userRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
