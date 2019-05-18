using System.Collections.Generic;
using GarbageCollector.Services.Impl;

namespace GarbageCollector.Services
{
    public interface IDataUploader
    {
        IEnumerable<ImportModel> Upload();
        void ImportCategories();
    }
}