using csharp_mfca.API.Features.Core;
using csharp_mfca.API.Features.Users.CreateUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csharp_mfca.API.Features.Users.Core
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly BL_CreateUser _bL_CreateUser;

        public UsersController(BL_CreateUser bL_CreateUser)
        {
            _bL_CreateUser = bL_CreateUser;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken cs)
        {
            var result = await _bL_CreateUser.CreateUserAsync(request, cs);
            return Content(result);
        }
    }
}
