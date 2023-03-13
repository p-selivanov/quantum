using Microsoft.AspNetCore.Mvc;

namespace CustomerGateway.Controllers;

[ApiController]
[Route("profile")]
public class ProfileController : ControllerBase
{
    public ProfileController()
    {
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok();
    }
}