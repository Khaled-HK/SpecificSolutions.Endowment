﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    // [Authorize] // Temporarily disabled for testing
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
    }
}
