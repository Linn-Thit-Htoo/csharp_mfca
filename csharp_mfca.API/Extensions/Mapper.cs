using csharp_mfca.API.Entities;
using csharp_mfca.API.Features.Users.CreateUser;

namespace csharp_mfca.API.Extensions
{
    public static class Mapper
    {
        public static TblUser ToEntity(this CreateUserRequest request, string hashPassword, string salt)
        {
            return new TblUser
            {
                UserId = Ulid.NewUlid().ToString(),
                FullName = request.FullName,
                Email = request.Email,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                PasswordHash = hashPassword,
                Salt = salt
            };
        }
    }
}
