using System.Threading.Tasks;
using GarbageCollector.Domain;
using GarbageCollector.Domain.Services;
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

        [Route("signup")]
        public async Task<IActionResult> BeginWorkAsync(UserViewModel user)
        {
            var userViewModel = (await workflowsService.CreateUserAsync(user).ConfigureAwait(true));
            if (userViewModel != null)
            {
                return Ok(userViewModel);
            }

            return BadRequest();
        }
    }
}