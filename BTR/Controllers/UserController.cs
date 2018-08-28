using Microsoft.AspNetCore.Mvc;

namespace BTR.Controllers
{
    [Produces("application/json")]
    [Route("user/")]
    public class UserController : Controller
    {
    //    private UNOSUserPrincipalService _userPrincipal;
        public UserController()
        {
            //_userPrincipal = userPrincipal;
        }

        [HttpGet]
        public ActionResult GetUserInfo()
        {
            return Ok();
        }
    }
}