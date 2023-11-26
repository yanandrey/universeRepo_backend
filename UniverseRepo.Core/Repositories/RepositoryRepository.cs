using Microsoft.EntityFrameworkCore;
using UniverseRepo.Application.DTOs.Request;
using UniverseRepo.Application.DTOs.Response;
using UniverseRepo.Application.Enums;
using UniverseRepo.Application.Models;
using UniverseRepo.Infra.Context;

namespace UniverseRepo.Core.Repositories;

public interface IRepositoryRepository
{
    Task<List<RepositoryResponseDto>> GetMineAsync(
        Guid id);

    Task<RepositoryResponseDto> GetByIdAsync(
        Guid id);
    
    Task<List<RepositoryResponseDto>> GetByNameAsync(
        string name);

    Task<RepositoryResponseDto> RegisterAsync(
        RepositoryRequestDto dto,
        Guid ownerId);

    Task<RepositoryResponseDto> UpdateAsync(
        RepositoryUpdateRequestDto dto);
    
    Task<RepositoryResponseDto> UpdateContentAsync(
        ContentUpdateRequestDto dto);

    Task DeleteAsync(
        Guid id);
}

public class RepositoryRepository : IRepositoryRepository
{
    private readonly ApiDbContext _context;

    public RepositoryRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<RepositoryResponseDto>> GetMineAsync(
        Guid id)
    {
        var repositoriesList = new List<RepositoryResponseDto>();

        var repositories = await _context.Repositories
            .Include(x => x.Owner)
            .Where(x => x.Owner.Id == id)
            .AsNoTracking()
            .Select(x => x.Id)
            .ToListAsync();
        
        foreach (var repository in repositories)
        {
            var repositoryToAdd = await GetByIdAsync(repository);
            repositoriesList.Add(repositoryToAdd);
        }
        
        return repositoriesList;
    }

    public async Task<RepositoryResponseDto> GetByIdAsync(Guid id)
    {
        var repository = await _context.Repositories
            .Include(x => x.Owner)
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        if (repository == null) throw new Exception("Repository not found");

        var contents = await _context.Contents
            .Where(x => x.Repository.Id == repository.Id)
            .AsNoTracking()
            .Select(x => new ContentResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Value = x.Value
            })
            .ToListAsync();
        
        return new RepositoryResponseDto
        {
            Id = repository.Id,
            Name = repository.Name,
            Description = repository.Description,
            Type = repository.Type,
            CreatedAt = repository.CreatedAt,
            OwnerId = repository.Owner.Id,
            Contents = contents
        };
    }

    public async Task<List<RepositoryResponseDto>> GetByNameAsync(string name)
    {
        var response = new List<RepositoryResponseDto>();
        
        var repositories = await _context.Repositories
            .Include(x => x.Owner)
            .Where(x => x.Type == RepositoryType.Public)
            .Where(x => x.Name.Contains(name))
            .AsNoTracking()
            .ToListAsync();

        foreach (var item in repositories)
        {
            var contents = await _context.Contents
                .Where(x => x.Repository.Id == item.Id)
                .AsNoTracking()
                .Select(x => new ContentResponseDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Value = x.Value
                })
                .ToListAsync();
        
            var repository = new RepositoryResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Type = item.Type,
                CreatedAt = item.CreatedAt,
                OwnerId = item.Owner.Id,
                Contents = contents
            };
            
            response.Add(repository);
        }

        return response;
    }

    public async Task<RepositoryResponseDto> RegisterAsync(
        RepositoryRequestDto dto,
        Guid ownerId)
    {
        var ownerToUse = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == ownerId);
        
        var repository = new Repository
        {
            Name = dto.Name,
            Description = dto.Description,
            Type = dto.Type,
            CreatedAt = DateTime.Now,
            Owner = ownerToUse
        };

        await _context.Repositories.AddAsync(repository);

        if (dto.Contents != null)
        {
            foreach (var item in dto.Contents)
            {
                var content = new Content
                {
                    Title = item.Title,
                    Value = item.Value,
                    Repository = repository
                };

                await _context.Contents.AddAsync(content);
            }
        }

        await _context.SaveChangesAsync();

        return await GetByIdAsync(repository.Id);
    }

    public async Task<RepositoryResponseDto> UpdateAsync(
        RepositoryUpdateRequestDto dto)
    {
        var repository = await _context.Repositories
            .FirstOrDefaultAsync(x => x.Id == dto.Id);
        
        if (repository == default) throw new Exception("Repository not found.");

        repository.Name = dto.Name;
        repository.Description = dto.Description;
        repository.Type = dto.Type;

        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(repository.Id);
    }

    public async Task<RepositoryResponseDto> UpdateContentAsync(
        ContentUpdateRequestDto dto)
    {
        var repository = await _context.Repositories
            .FirstOrDefaultAsync(x => x.Id == dto.Id);
        
        if (repository == default) throw new Exception("Repository not found.");

        var contents = await _context.Contents
            .Include(x => x.Repository)
            .Where(x => x.Repository.Id == dto.Id)
            .ToListAsync();

        foreach (var item in dto.Contents)
        {
            var alreadyExists = await _context.Contents
                .Include(x => x.Repository)
                .Where(x => x.Repository.Id == dto.Id)
                .Where(x => x.Title == item.Title)
                .FirstOrDefaultAsync();

            if (alreadyExists == default)
            {
                var content = new Content
                {
                    Title = item.Title,
                    Value = item.Value,
                    Repository = repository
                };

                await _context.Contents.AddAsync(content);
            }
        }

        foreach (var item in contents)
        {
            var stillExists = dto.Contents
                .FirstOrDefault(x => x.Title == item.Title);

            if (stillExists == default)
            {
                var content = await _context.Contents
                    .Include(x => x.Repository)
                    .Where(x => x.Repository.Id == dto.Id)
                    .Where(x => x.Title == item.Title)
                    .FirstOrDefaultAsync();

                _context.Contents.Remove(content);
            }
        }

        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(repository.Id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var repository = await _context.Repositories
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (repository == default) throw new Exception("Repository not found.");

        var contents= await _context.Contents
            .Include(x => x.Repository)
            .Where(x => x.Repository.Id == repository.Id)
            .ToListAsync();
        
        _context.Contents.RemoveRange(contents);
        _context.Repositories.Remove(repository);
        await _context.SaveChangesAsync();
    }
}