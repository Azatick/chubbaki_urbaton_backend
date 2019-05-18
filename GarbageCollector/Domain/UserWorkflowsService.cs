using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Controllers;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Extensions;
using GarbageCollector.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Domain
{
    public class UserWorkflowsService
    {
        private IMapper mapper;
        private readonly GarbageCollectorContext dbContext;

        public UserWorkflowsService(IMapper mapper, GarbageCollectorContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<bool> TryCreateUserAsync(UserViewModel createModel)
        {
            var user = mapper.Map<GarbageAppUser>(createModel);
            if (!(await CheckIsPossibleCreateUserAsync(user).ConfigureAwait(false)))
            {
                return false;
            }
            await user.UpdateTrashCansByCurrentLocationAsync().ConfigureAwait(false);
            var dbUser = mapper.Map<GarbageAppUserDbo>(user);
            await dbContext.AppUsers.AddAsync(dbUser).ConfigureAwait(false);
            return true;
        }

        private async Task<bool> CheckIsPossibleCreateUserAsync(GarbageAppUser user) =>
            !user.Login.IsNullOrEmpty() &&
            (await dbContext.AppUsers.AnyAsync(dbuser => dbuser.Login == user.Login)
                .ConfigureAwait(false));
    }
}