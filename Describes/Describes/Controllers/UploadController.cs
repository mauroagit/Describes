using System;
using System.IO;
using System.IO.Compression;
using Describes.Services;
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
            string pathToWavFile = "";
            string path = "";

            using (ZipArchive archive = new ZipArchive(new MemoryStream(bytes)))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string extension = Path.GetExtension(entry.FullName);
                    if (!string.IsNullOrEmpty(extension)) //make sure it's not a folder
                    {
                        path = Path.Combine(uploads, entry.FullName);
                        entry.ExtractToFile(path, true);
                        if (extension.ToLower() == ".wav")
                        {
                            pathToWavFile = path;
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.Combine(uploads, entry.FullName));
                    }
                }


                // Call service
                // "C:\\Uploads\\24375.jpg"
                var speechServices = new SpeechService();
                var text = speechServices.RecognizeSpeechFromFileAsync(pathToWavFile);

            }

            return Ok();
        }
    }
}
