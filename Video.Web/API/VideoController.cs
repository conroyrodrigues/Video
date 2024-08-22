using Microsoft.AspNetCore.Mvc;
using Video.Web.Pages;
using Video.Web.Services;
using Video.Web.Transport;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Video.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        public IEnumerable<Transport.Video> Videos { get; set; }

        [BindProperty]
        public List<IFormFile> VideoFiles { get; set; }

        private readonly IWebHostEnvironment _env;
        private readonly string _mediaFolderPath;
        private IVideoService _service;


        public VideoController(IWebHostEnvironment env, IVideoService service)
        {
            _env = env;
            _service = service;
            _mediaFolderPath = Path.Combine(_env.WebRootPath, "media");

            if (!Directory.Exists(_mediaFolderPath))
            {
                Directory.CreateDirectory(_mediaFolderPath);
            }
        }


        // GET: api/<VideoController>
        [HttpGet]
        public ActionResult<Transport.Video> Get()
        {
            Videos = Directory
                 .GetFiles(_mediaFolderPath)
                 .Select(filePath => new Transport.Video
                 {
                     FileName = Path.GetFileName(filePath),
                     FilePath = "/media/" + Path.GetFileName(filePath),
                     FileSize = new FileInfo(filePath).Length / 1024 / 1024
                 })
                 .ToList();
            
            return Ok(Videos);
        }

        // POST api/<VideoController>
        [HttpPost("upload")]
        [RequestSizeLimit(200_000_000)] 
        public async Task<IActionResult> Upload(IFormFile[] files)
        {
            var response = new List<CatalogResponse>();
            var errorMessages = new List<string>();

            await _service.PostMultiFileAsync(
                 files,
                 Enums.SourceType.API,
                 _env.WebRootPath,
                 errorMessages);

            if (errorMessages.Count > 0)
            {
                response.Add(new CatalogResponse
                {
                    Type = "Error",
                    Message = errorMessages
                });
            }
            else
            {
                response.Add(new CatalogResponse
                {
                    Type = "Success",
                    Message = new List<string> { "Media Uploaded successfully" }
                });
            }
            return Ok(response);
        }
    }
}
