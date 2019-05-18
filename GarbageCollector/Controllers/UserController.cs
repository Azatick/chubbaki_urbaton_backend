using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        [Route("signup")]
        public IActionResult BeginWork(UserViewModel user)
        {
            return
            View();
        }
        
        
    }

    public class UserViewModel
    {
        
    }
}