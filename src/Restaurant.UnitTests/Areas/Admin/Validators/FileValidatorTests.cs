using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Restaurant.Areas.Admin.Validators;

namespace Restaurant.UnitTests.Areas.Admin.Validators
{
    public class FileValidatorTests
    {
        private const string LONG_FILE_NAME = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.jpg";

        [Fact]
        public void IsValid_NullFile_ReturnsFalse()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var fileValidator = new FileValidator(configuration);

            // Act
            var result = fileValidator.IsValid(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ValidFileData))]
        public void IsValid_ValidFile_ReturnsTrue(int fileSize, string fileName)
        {
            // Arrange
            var bytes = new byte[fileSize]; // Encoding.UTF8.GetBytes("Test");
            var formFile = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", fileName);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var fileValidator = new FileValidator(configuration);

            // Act
            var result = fileValidator.IsValid(formFile);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(InvalidFileData))]
        public void IsValid_InvalidFile_ReturnsFalse(int fileSize, string fileName)
        {
            // Arrange
            var bytes = new byte[fileSize]; // Encoding.UTF8.GetBytes("Test");
            var formFile = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", fileName);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var fileValidator = new FileValidator(configuration);

            // Act
            var result = fileValidator.IsValid(formFile);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> ValidFileData => new List<object[]>()
        {
            new object[] { 500 * 1024, "file.jpg" },
            new object[] { 500 * 1024, "file.jpeg" },
            new object[] { 500 * 1024, "file.png" }
        };

        public static IEnumerable<object[]> InvalidFileData => new List<object[]>()
        {
            // Invalid file name/extension
            new object[] { 500 * 1024, string.Empty },
            new object[] { 500 * 1024, LONG_FILE_NAME },
            new object[] { 500 * 1024, "file" },
            new object[] { 500 * 1024, "file.txt" },

            // Invalid file size
            new object[] { 2 * 1024 * 1024, "file.jpg" },
            new object[] { 2 * 1024 * 1024, "file.jpeg" },
            new object[] { 2 * 1024 * 1024, "file.png" }
        };
    }
}
