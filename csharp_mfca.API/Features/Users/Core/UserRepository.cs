using csharp_mfca.API.Entities;
using csharp_mfca.API.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace csharp_mfca.API.Features.Users.Core
{
    public class UserRepository : RepositoryBase<TblUser>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}
