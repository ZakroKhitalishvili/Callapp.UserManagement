using Callapp.UserManagement.Application.Dtos;
using Callapp.UserManagement.Application.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Interfaces;

public interface IUserService
{
    Task<ServiceResult<UserDto>> CreateAsync(UserCreateDto userCreate, CancellationToken cancellationToken);
    Task<ServiceResult> UpdateAsync(UserUpdateDto userUpdate, CancellationToken cancellationToken);
    Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<ServiceResult<UserDto>> GetAsync(int id, CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<UserDto>>> GetAllAsync( CancellationToken cancellationToken);
    Task<ServiceResult<UserDto>> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken);
}
