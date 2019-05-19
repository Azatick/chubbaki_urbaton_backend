using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GarbageCollector.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace GarbageCollector.Controllers
{
    public class TestController : Controller
    {
        private IDataUploader _dataUploader;

        public TestController(IDataUploader dataUploader)
        {
            _dataUploader = dataUploader;
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
        [CanBeNull] [HttpGet("/makePointsToCatsLinks")]
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