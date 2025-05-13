using csharp_mfca.API.Constants;
using csharp_mfca.API.Entities;
using csharp_mfca.API.Extensions;
using csharp_mfca.API.Persistence.Wrapper;
using csharp_mfca.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace csharp_mfca.API.Features.Users.CreateUser
{
    public class DA_CreateUser(IUnitOfWork unitOfWork, IPasswordHasher<TblUser> passwordHasher, IHttpContextAccessor httpContextAccessor)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher<TblUser> _passwordHasher = passwordHasher;
        private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

        public async Task<BaseResponse<CreateUserResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cs = default)
        {
            BaseResponse<CreateUserResponse> result;

            bool emailExist = await _unitOfWork.UserRepository
                .GetByCondition(x => x.Email == request.Email.Trim())
                .AnyAsync(cancellationToken: cs);
            if (emailExist)
            {
                result = BaseResponse<CreateUserResponse>.Fail(_httpContext.TraceIdentifier, MessageConstant.EmailDuplicate, HttpStatusCode.Conflict);
                goto result;
            }

            string salt = Guid.NewGuid().ToString("N");
            var hashedPassword = _passwordHasher.HashPassword(null!, $"{request.Password}{salt}");

            await _unitOfWork.UserRepository.AddAsync(request.ToEntity(hashedPassword, salt), cs);
            await _unitOfWork.SaveChangesAsync(cs);

            result = BaseResponse<CreateUserResponse>.Success(_httpContext.TraceIdentifier);

        result:
            return result;
        }
    }
}
