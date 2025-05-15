using csharp_mfca.API.Entities;
using csharp_mfca.API.Features.Users.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace csharp_mfca.API.Persistence.Wrapper;

public class UnitOfWork : IUnitOfWork
{
    internal readonly DbContext _context;
    internal readonly string? _currentUser = "SYSTEM";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = dbContext;

        if (_httpContextAccessor.HttpContext is not null)
        {
            if (_httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            {
                _currentUser = _httpContextAccessor
                    .HttpContext.User.Identities.FirstOrDefault()!
                    .Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault()!
                    .Value;
            }
        }

        UserRepository = new UserRepository(_context);
    }

    public void SaveChanges()
    {
        var modifiedEntries = _context
            .ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            Type type = entry.Entity.GetType();

            if (entry.State == EntityState.Added)
            {
                PropertyInfo createdBy = type.GetProperty("created_by")!;
                createdBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo createdDate = type.GetProperty("created_at")!;
                createdDate?.SetValue(entry.Entity, DateTime.Now);
            }

            if (entry.State == EntityState.Modified)
            {
                PropertyInfo modifiedBy = type.GetProperty("updated_by")!;
                modifiedBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo modifiedAt = type.GetProperty("updated_at")!;
                modifiedAt?.SetValue(entry.Entity, DateTime.Now);
            }
        }

        _context.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cs = default)
    {
        var modifiedEntries = _context
            .ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            Type type = entry.Entity.GetType();

            if (entry.State == EntityState.Added)
            {
                PropertyInfo createdBy = type.GetProperty("created_by")!;
                createdBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo createdDate = type.GetProperty("created_at")!;
                createdDate?.SetValue(entry.Entity, DateTime.Now);
            }

            if (entry.State == EntityState.Modified)
            {
                PropertyInfo modifiedBy = type.GetProperty("updated_by")!;
                modifiedBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo modifiedAt = type.GetProperty("updated_at")!;
                modifiedAt?.SetValue(entry.Entity, DateTime.Now);
            }
        }

        await _context.SaveChangesAsync(cs);
    }

    public IUserRepository UserRepository { get; set; }
}
