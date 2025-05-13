using csharp_mfca.API.Utils;
using FluentValidation;

namespace csharp_mfca.API.Features.Users.CreateUser
{
    public class BL_CreateUser(DA_CreateUser dA_CreateUser, IValidator<CreateUserRequest> validator, IHttpContextAccessor httpContextAccessor)
    {
        private readonly DA_CreateUser _dA_CreateUser = dA_CreateUser;
        private readonly IValidator<CreateUserRequest> _validator = validator;
        private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

        public async Task<BaseResponse<CreateUserResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cs = default)
        {
            BaseResponse<CreateUserResponse> result;

            var validationResult = await _validator.ValidateAsync(request, cs);
            if (!validationResult.IsValid)
            {
                result = BaseResponse<CreateUserResponse>.Fail(_httpContext.TraceIdentifier,string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage)));
                goto result;
            }

            result = await _dA_CreateUser.CreateUserAsync(request, cs);

        result:
            return result;
        }
    }
}
