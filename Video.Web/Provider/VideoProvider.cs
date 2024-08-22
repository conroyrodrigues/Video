using System.Collections.Generic;
using Video.Web.Enums;
using Video.Web.Services;
using Video.Web.Transport;
using System.Linq;

namespace Video.Web.Provider
{
    public class VideoProvider : IVideoService
    {
        private const long _MaxFileSize = 200 * 1024 * 1024; // 200MB: Applicable to API only

        Func<IEnumerable<IFormFile>, bool> hasFilesToCopy = files => { return files.Any(); };

        Func<IEnumerable<IFormFile>, bool> isValidFormat = files => { return files.Any(a => a.ContentType == "video/mp4"); };

        Func<IFormFile[], bool> isValidLength = file => { return file.Length < _MaxFileSize; };


        public async Task PostMultiFileAsync(IEnumerable<IFormFile> files, SourceType sourceType, string webPath, List<string> errorMessage)
        {
            if (!hasFilesToCopy(files))
            {
                errorMessage.Add("No files Selected");
            }
            if (!isValidFormat(files))
            {
                errorMessage.Add("File Not .mp4 Format");
            }

            if (sourceType == SourceType.API && !isValidLength(files.ToArray()))
            {
               errorMessage.Add("File size too large");
            }
            
            if (!errorMessage.Any()) 
            {
                foreach (var file in files)
                {
                    var filePath = Path.Combine(webPath, "media", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

        }
    }
}
