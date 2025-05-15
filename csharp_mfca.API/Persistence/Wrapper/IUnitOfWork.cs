using csharp_mfca.API.Features.Users.Core;

namespace csharp_mfca.API.Persistence.Wrapper;

public interface IUnitOfWork
{
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cs = default);
    IUserRepository UserRepository { get; }
}
