using csharp_mfca.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace csharp_mfca.API.Features.Core;

public class BaseController : ControllerBase
{
    protected IActionResult Content(object obj)
    {
        return Content(obj.ToJson(), "application/json");
    }
}
