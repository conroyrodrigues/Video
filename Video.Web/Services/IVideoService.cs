using Microsoft.VisualBasic.FileIO;
using Video.Web.Enums;

namespace Video.Web.Services
{
    public interface IVideoService
    {
        public Task PostMultiFileAsync(IEnumerable<IFormFile> files, SourceType sourceType, string webPath, List<string> errorMessage);
    }
}
