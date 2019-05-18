using System;
using System.Globalization;
using System.Linq;
using GarbageCollector.Services;
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
    }
}