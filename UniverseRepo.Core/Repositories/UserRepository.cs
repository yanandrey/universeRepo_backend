using Microsoft.EntityFrameworkCore;
using UniverseRepo.Application.DTOs.Request;
using UniverseRepo.Application.DTOs.Response;
using UniverseRepo.Application.Enums;
using UniverseRepo.Application.Helpers;
using UniverseRepo.Application.Models;
using UniverseRepo.Infra.Context;

namespace UniverseRepo.Core.Repositories;

public interface IUserRepository
{
    Task<UserResponseDto> GetMeAsync(
        Guid id);
    
    Task<UserResponseDto> GetByIdAsync(
        Guid id);
    
    Task<UserResponseDto> RegisterAsync(
        UserRequestDto dto);
    
    Task<UserResponseDto> UpdateAsync(
        UserUpdateRequestDto dto);
    
    Task DisableAsync(
        Guid id);
    
    Task<User> AuthenticateAsync(
        LoginRequestDto dto);
}

public class UserRepository : IUserRepository
{
    private readonly ApiDbContext _context;

    public UserRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponseDto> GetMeAsync(
        Guid id)
    {
        var user = await _context.Users
            .Where(x => x.Id == id)
            .AsNoTracking()
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        return await GetByIdAsync(user);
    }

    public async Task<UserResponseDto> GetByIdAsync(Guid id)
    {
        var user = await _context.Users
            .Include(x => x.Account)
            .Where(x => x.Id == id)
            .Where(x => x.Account.Status == AccountStatus.Actived)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        if (user == null) throw new Exception("User not found");
        
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            MemberSince = user.Account.CreatedAt
        };
    }

    public async Task<UserResponseDto> RegisterAsync(
        UserRequestDto dto)
    {
        var isEmailAvailable = await CheckIfEmailAlreadyExistsAsync(dto.Email);
        if (isEmailAvailable) throw new Exception("Email already in use");
        
        var userAccount = new UserAccount
        {
            Token = RandomStringHelper.RandomString(8),
            Status = AccountStatus.Actived,
            CreatedAt = DateTime.Now
        };
        
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            Password = HashingHelper.HashPassword(dto.Password),
            Account = userAccount
        };
        
        await _context.UserAccounts.AddAsync(userAccount);
        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(user.Id);
    }
    
    public async Task<UserResponseDto> UpdateAsync(UserUpdateRequestDto dto)
    {
        var user = await _context.Users
            .Include(x => x.Account)
            .Where(x => x.Id == dto.Id)
            .Where(x => x.Account.Status == AccountStatus.Actived)
            .FirstOrDefaultAsync();
        
        if (user == default) throw new Exception("User not found.");
        
        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.BirthDate = dto.BirthDate;
        user.Gender = dto.Gender;
        
        await _context.SaveChangesAsync();
        return await GetByIdAsync(user.Id);
    }
    
    public async Task DisableAsync(Guid id)
    {
        var user = await _context.Users
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (user == default) throw new Exception("User not found.");

        user.Account.Status = AccountStatus.Inactived;
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> AuthenticateAsync(
        LoginRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new Exception("Username or password is empty");
        }
        
        var user = await _context.Users
            .Include(x => x.Account)
            .Where(x => x.Email == dto.Email)
            .Where(x => x.Account.Status == AccountStatus.Actived)
            .FirstOrDefaultAsync();
        
        if (user?.Password == null || !HashingHelper.ValidatePassword(dto.Password, user.Password))
        {
            throw new Exception("Invalid credentials.");
        }
        
        return user;
    }
    
    private async Task<bool> CheckIfEmailAlreadyExistsAsync(
        string email)
    {
        var alreadyExists = await _context.Users
            .AnyAsync(x => x.Email == email);
        
        return alreadyExists;
    }
}