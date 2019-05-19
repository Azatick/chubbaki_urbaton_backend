using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GarbageCollector.Domain;
using GarbageCollector.Domain.Services;
using GarbageCollector.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers
{
    [ApiController]
    [Route("user"),EnableCors("MyPolicy")]
    public class UserController : Controller
    {
        private readonly UserWorkflowsService workflowsService;

        public UserController(UserWorkflowsService workflowsService)
        {
            this.workflowsService = workflowsService;
        }

        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> BeginWorkAsync(UserViewModel user)
        {
            var userViewModel = (await workflowsService.CreateUserAsync(user).ConfigureAwait(true));
            if (userViewModel != null)
            {
                return Ok(userViewModel);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser([FromQuery][Required(AllowEmptyStrings = false)] string userLogin)
        {
            var user = await workflowsService.GetUserAsync(userLogin).ConfigureAwait(true);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }
    }
}