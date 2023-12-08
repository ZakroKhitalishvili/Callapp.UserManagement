using Callapp.UserManagement.Application.Interfaces;
using Callapp.UserManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Callapp.UserManagement.Data.Models;
using FluentValidation;
using Callapp.UserManagement.Application.Validators;
using Microsoft.EntityFrameworkCore;
using Callapp.UserManagement.Application.Dtos.Users;

namespace Callapp.UserManagement.Application.Services;

public class UserService : IUserService
{
    private readonly CallappContext _context;

    public UserService(CallappContext context)
    {
        _context = context;
    }

    private string HashString(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            return String.Empty;
        }

        // Uses SHA256 to create the hash
        using var sha = SHA512.Create();
        // Convert the string to a byte array first, to be processed
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] hashBytes = sha.ComputeHash(textBytes);

        // Convert back to a string, removing the '-' that BitConverter adds
        string hash = BitConverter
            .ToString(hashBytes)
            .Replace("-", String.Empty);

        return hash;
    }


    public async Task<ServiceResult<UserDto>> LoginAsync(UserLoginDto userLoginDto, CancellationToken cancellationToken)
    {
        var validator = new UserLoginValidator();

        var validatorResult = validator.Validate(userLoginDto);

        if (!validatorResult.IsValid)
        {
            return ServiceResult<UserDto>.Error(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        var hashedPassword = HashString(userLoginDto.Password);
        var user = await _context.Users.Where(x => x.Email == userLoginDto.Email && x.Password == hashedPassword)
            .Include(x => x.UserProfile).SingleOrDefaultAsync();

        if (user is null)
        {
            return ServiceResult<UserDto>.Error("Email or password is wrong");
        }

        return ServiceResult<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Firstname = user.UserProfile.Firstname,
            Lastname = user.UserProfile.Lastname,
            PersonalNumber = user.UserProfile.PersonalNumber,
            Username = user.Username,
            IsActive = user.IsActive,
        });
    }

    public async Task<ServiceResult<UserDto>> CreateAsync(UserCreateDto userCreate, CancellationToken cancellationToken)
    {

        var validator = new UserCreateValidator();

        var validatorResult = await validator.ValidateAsync(userCreate, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return ServiceResult<UserDto>.Error(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        var dublicateUser = await _context.Users
            .Where(x => x.Username == userCreate.Username || x.Email == userCreate.Email)
            .SingleOrDefaultAsync();

        if (dublicateUser is not null)
        {
            return ServiceResult<UserDto>.Error("Username/Email already has been used");
        }

        var user = await _context.Users.AddAsync(new User
        {
            Email = userCreate.Email,
            IsActive = true,
            Password = HashString(userCreate.Password),
            Username = userCreate.Username,
            UserProfile = new UserProfile
            {
                Firstname = userCreate.Firstname,
                Lastname = userCreate.Lastname,
                PersonalNumber = userCreate.PersonalNumber,
            }
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);


        return ServiceResult<UserDto>.Success(new UserDto
        {
            Id = user.Entity.Id,
            Email = user.Entity.Email,
            Firstname = user.Entity.UserProfile.Firstname,
            Lastname = user.Entity.UserProfile.Lastname,
            PersonalNumber = user.Entity.UserProfile.PersonalNumber,
            Username = user.Entity.Username,
        });
    }

    public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var rows = await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);

        if (rows > 0)
        {
            return ServiceResult.Success();
        }

        return ServiceResult.Error("Failed to delete an user");
    }

    public async Task<ServiceResult<IReadOnlyList<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users.Include(x => x.UserProfile).ToListAsync(cancellationToken);

        if (users is null)
        {
            return ServiceResult<IReadOnlyList<UserDto>>.Error("Users not found");
        }

        return ServiceResult<IReadOnlyList<UserDto>>.Success(users.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Firstname = user.UserProfile.Firstname,
            Lastname = user.UserProfile.Lastname,
            PersonalNumber = user.UserProfile.PersonalNumber,
            Username = user.Username,
            IsActive = user.IsActive,
        }).ToList());
    }

    public async Task<ServiceResult<UserDto>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(x => x.UserProfile).SingleOrDefaultAsync(x => x.Id == id);

        if (user is null)
        {
            return ServiceResult<UserDto>.Error("User not found");
        }

        return ServiceResult<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Firstname = user.UserProfile.Firstname,
            Lastname = user.UserProfile.Lastname,
            PersonalNumber = user.UserProfile.PersonalNumber,
            Username = user.Username,
            IsActive = user.IsActive,
        });
    }

    public async Task<ServiceResult> UpdateAsync(UserUpdateDto userUpdate, CancellationToken cancellationToken)
    {
        var validator = new UserUpdateValidator();

        var validatorResult = await validator.ValidateAsync(userUpdate, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return ServiceResult.Error(validatorResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        var user = await _context.Users.Where(x => x.Id == userUpdate.Id)
            .Include(x => x.UserProfile)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return ServiceResult.Error("User not found");
        }

        var dublicateUser = await _context.Users
            .Where(x => (x.Username == userUpdate.Username || x.Email == userUpdate.Email) && x.Id != userUpdate.Id)
            .SingleOrDefaultAsync();

        if (dublicateUser is not null)
        {
            return ServiceResult<UserDto>.Error("Username/Email already has been used");
        }

        user.Email = userUpdate.Email;
        user.Username = userUpdate.Username;
        user.IsActive = userUpdate.IsActive!.Value;
        user.UserProfile.Firstname = userUpdate.Firstname;
        user.UserProfile.Lastname = userUpdate.Lastname;
        user.UserProfile.PersonalNumber = userUpdate.PersonalNumber;

        if (userUpdate.NewPassword is not null || userUpdate.ConfirmNewPassword is not null)
        {
            var newPassword = HashString(userUpdate.NewPassword!);
            if (newPassword.Equals(user.Password))
                return ServiceResult.Error("New password should not be match an existing password");
            user.Password = HashString(userUpdate.NewPassword!);
        }

        int rows = await _context.SaveChangesAsync(cancellationToken);

        if (rows > 0)
        {
            return ServiceResult.Success();
        }

        return ServiceResult.Error("Failed to update an user");
    }

}
