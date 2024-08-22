using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Video.Web.Enums;
using Video.Web.Provider;

namespace Video.Tests.Provider
{
    public class VideoProviderTests
    {
        private readonly VideoProvider _provider;

        public VideoProviderTests()
        {
            _provider = new VideoProvider();
        }

        [Fact]
        public async Task PostMultiFileAsync_NoFiles_Selected_ShouldAddError()
        {
            // Arrange
            var errorMessage = new List<string>();
            var files = Enumerable.Empty<IFormFile>();

            // Act
            await _provider.PostMultiFileAsync(files, SourceType.API, "path", errorMessage);

            // Assert
            Assert.Contains("No files Selected", errorMessage);
        }

        [Fact]
        public async Task PostMultiFileAsync_InvalidFileFormat_ShouldAddError()
        {
            // Arrange
            var errorMessage = new List<string>();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns("video/avi");
            var files = new[] { fileMock.Object };

            // Act
            await _provider.PostMultiFileAsync(files, SourceType.API, "path", errorMessage);

            // Assert
            Assert.Contains("File Not .mp4 Format", errorMessage);
        }

    }
}
