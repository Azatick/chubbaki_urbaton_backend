using System.Threading.Tasks;
using GarbageCollector.Domain;
using GarbageCollector.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserWorkflowsService workflowsService;

        public UserController(UserWorkflowsService workflowsService)
        {
            this.workflowsService = workflowsService;
        }

        //[Route("signup")]
        public async Task<IActionResult> BeginWorkAsync(UserViewModel user)
        {
            if (await workflowsService.TryCreateUserAsync(user).ConfigureAwait(true))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}