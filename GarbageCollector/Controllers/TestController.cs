using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers
{
    public class TestController : Controller
    {
        private IDataUploader _dataUploader;
        private GarbageCollectorContext garbageCollectorContext;

        public TestController(IDataUploader dataUploader, GarbageCollectorContext garbageCollectorContext)
        {
            _dataUploader = dataUploader;
            this.garbageCollectorContext = garbageCollectorContext;
        }

        [HttpGet("/updateAll")]
        public async Task<IActionResult> PrepareDb()
        {
            garbageCollectorContext.AppUsers.RemoveRange(garbageCollectorContext.AppUsers);
            garbageCollectorContext.WasteTakePoints.RemoveRange(garbageCollectorContext.WasteTakePoints);
            garbageCollectorContext.WasteCategories.RemoveRange(garbageCollectorContext.WasteCategories);
            garbageCollectorContext.SaveChanges();

            var points = _dataUploader.Upload();
            _dataUploader.ImportCategories();
            await _dataUploader.MapPointsToCategoriesAsync().ConfigureAwait(true);
            await _dataUploader.CreateDefaultUser().ConfigureAwait(true);
            return Ok();
        }

        [HttpGet("/upload")]
        public IActionResult Index()
        {
            var points = _dataUploader.Upload();

            return Json(points.Take(20));
        }

        [HttpGet("/uploadCats")]
        public IActionResult UploadCats()
        {
            _dataUploader.ImportCategories();

            return Ok();
        }

        [CanBeNull]
        [HttpGet("/makePointsToCatsLinks")]
        public async Task<IActionResult> MakePointsToCatsLinks()
        {
            await _dataUploader.MapPointsToCategoriesAsync().ConfigureAwait(true);

            return Ok();
        }

        [HttpGet("/createDefaultUser")]
        public async Task<IActionResult> CreateDefUserAsync()
        {
            await _dataUploader.CreateDefaultUser().ConfigureAwait(true);

            return Ok("Pupkin");
        }
    }
}