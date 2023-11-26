using Microsoft.EntityFrameworkCore;
using UniverseRepo.Application.Models;

namespace UniverseRepo.Infra.Context;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Content> Contents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
        
        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
        
        modelBuilder.Entity<Repository>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
        
        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
    }
}