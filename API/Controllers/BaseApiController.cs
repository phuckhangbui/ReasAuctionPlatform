using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected int GetLoginAccountId()
        {
            try
            {
                return int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
