using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment hostingEnv;

        public HomeController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }        
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {


            string webRootPath = hostingEnv.WebRootPath;

            DirectoryInfo location = new DirectoryInfo(webRootPath + "/images/Uploaded");
            // var imagePath = Server.MapPath("~/wwwroot/images/Uploaded");
            var images = location.GetFiles();

            // foreach(var file in location.GetFiles()) {
            //     System.Console.WriteLine(file);
            // }
            ViewBag.files = images;

            ViewBag.Success = TempData["Success"];
            return View();
        }

        [HttpPost]
        [Route("UploadPhoto")]
        public IActionResult UploadPhoto(IList<IFormFile> Images) {
            
            long size = 0;
            var location = "";

            foreach(var file in Images) {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                location = $@"/images/uploaded/{filename}";
                filename = hostingEnv.WebRootPath + $@"\images\uploaded\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename)){
                file.CopyTo(fs);
                fs.Flush();
                }
            }

            TempData["Success"] = $"{Images.Count} files(s) uploaded successfuly!";
            return RedirectToAction("Index");
        }        
    }
}
