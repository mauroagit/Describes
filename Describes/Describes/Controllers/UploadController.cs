using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Describes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public UploadController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(FileModel file)
        {
            var uploads = configuration.GetValue<string>("AppConfiguration:Uploads");
            var bytes = Convert.FromBase64String(file.File);
            using (ZipArchive archive = new ZipArchive(new MemoryStream(bytes)))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (!string.IsNullOrEmpty(Path.GetExtension(entry.FullName))) //make sure it's not a folder
                    {
                        entry.ExtractToFile(Path.Combine(uploads, entry.FullName));
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.Combine(uploads, entry.FullName));
                    }
                }
            }

            return Ok();
        }
    }
}
