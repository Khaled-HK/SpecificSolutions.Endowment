using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [Authorize] // تفعيل الحماية على جميع Controllers
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
    }
}
