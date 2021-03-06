using System;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Extensions;
using GarbageCollector.ViewModels;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GarbageCollector.Domain.Services
{
    public class UserWorkflowsService
    {
        private readonly IMapper mapper;
        private readonly GarbageCollectorContext dbContext;
        private readonly CategoriesService categoriesService;
        private readonly WasteTakePointService wasteTakePointService;

        public UserWorkflowsService(IMapper mapper, GarbageCollectorContext dbContext, CategoriesService
            categoriesService, WasteTakePointService wasteTakePointService)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.categoriesService = categoriesService;
            this.wasteTakePointService = wasteTakePointService;
        }

        public async Task<UserViewModel> CreateUserAsync(UserViewModel createModel)
        {
            var user = mapper.Map<GarbageAppUser>(createModel);
            if (!(await CheckIsPossibleCreateUserAsync(user).ConfigureAwait(false)))
            {
                return null;
            }

            user.AddServices(categoriesService, wasteTakePointService);
            await user.UpdateTrashCansByCurrentLocationAsync().ConfigureAwait(false);
            var dbUser = mapper.Map<GarbageAppUserDbo>(user);
            await dbContext.AppUsers.AddAsync(dbUser).ConfigureAwait(false);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return mapper.Map<UserViewModel>(user);
        }

        private async Task<bool> CheckIsPossibleCreateUserAsync(GarbageAppUser user) =>
            !(user.Login.IsNullOrEmpty() || user.Id == default(Guid) &&
              (await dbContext.AppUsers.AnyAsync(dbuser => dbuser.Login == user.Login || dbuser.Id == user.Id)
                  .ConfigureAwait(false)));

        public async Task<UserViewModel> GetUserAsync([NotNull] string userLogin)
        {
            var userDbo = await dbContext.AppUsers.Include(u => u.CurrentLocation)
                .Include(u => u.TrashCans).ThenInclude(tc => tc.LinksToCategories).ThenInclude(l => l.Category)
                .Include(u => u.TrashCans).ThenInclude(tc => tc.WasteTakePoint)
                .FirstOrDefaultAsync(u => u.Login == userLogin).ConfigureAwait(false);
            if (userDbo == null)
            {
                return null;
            }

            var user = mapper.Map<GarbageAppUser>(userDbo);
            return mapper.Map<UserViewModel>(user);
        }
    }
}