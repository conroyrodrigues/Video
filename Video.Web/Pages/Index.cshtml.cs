using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Video.Web.Services;
using Video.Web.Transport;

namespace Video.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Transport.Video> Videos { get; set; }

        [BindProperty]
        public List<IFormFile> VideoFiles { get; set; }

        public string Message { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string _mediaFolderPath;
        private IVideoService _service;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env, IVideoService service)
        {
            _logger = logger;
            _env = env;
            _service = service;
            _mediaFolderPath = Path.Combine(_env.WebRootPath, "media");
            
            if (!Directory.Exists(_mediaFolderPath))
            {
                Directory.CreateDirectory(_mediaFolderPath);
            }

            Videos = Directory
                .GetFiles(_mediaFolderPath)
                .Select(filePath => new Transport.Video
                {
                    FileName = Path.GetFileName(filePath),
                    FilePath = "/media/" + Path.GetFileName(filePath),
                    FileSize = new FileInfo(filePath).Length / 1024 / 1024
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = new List<CatalogResponse>();
            var errorMessages = new List<string>(); 

             await _service.PostMultiFileAsync(
                 VideoFiles,
                 Enums.SourceType.Web, 
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

            return Redirect("/");
        }
    }
}
