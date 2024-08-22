using Microsoft.AspNetCore.Mvc;

namespace Video.Web.Transport
{
    public class Video
    {
        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public long FileSize { get; set; }
    }
}
