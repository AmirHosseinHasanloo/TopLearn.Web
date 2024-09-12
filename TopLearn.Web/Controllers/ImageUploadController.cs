using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using TopLearn.Core.Generators;

namespace TopLearn.Web.Controllers
{
    public class ImageUploadController : Controller
    {
        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/CkImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/CkImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }
    }
}
