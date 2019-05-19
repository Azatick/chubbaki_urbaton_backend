using System.Collections.Generic;
using System.Threading.Tasks;
using GarbageCollector.Services.Impl;

namespace GarbageCollector.Services
{
    public interface IDataUploader
    {
        IEnumerable<ImportModel> Upload();
        void ImportCategories();
        Task MapPointsToCategoriesAsync();
        Task CreateDefaultUser();
    }
}