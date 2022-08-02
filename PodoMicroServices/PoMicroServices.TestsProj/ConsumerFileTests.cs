using PodoMicroServices.Common.Dto.FileDto;
using PodoMicroServices.Consumer;
using System.Text;

namespace PoMicroServices.TestsProj
{
    public class ConsumerFileTests
    {
        public PodoConsumer Consumer { get; set; }

        public ConsumerFileTests()
        {
            Consumer = new PodoConsumer("https://localhost:1312");
            var result = Consumer.Login("TestUser", "TestUser", 3).Result;
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetFiles()
        {
            var result = await Consumer.GetFiles();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);

        }

        [Fact]
        public async Task FullCircleSecret()
        {
            var result = await Consumer.AddFile(new FileDto()
            {

                Name = "TestSecret",
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes("Test")),
                Alt = "Test",
                Created = DateTime.Now
            });
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            var checks = await Consumer.GetFiles();
            Assert.NotNull(checks);
            Assert.True(checks.Success);
            Assert.NotNull(checks.Data);
            if (checks.Data is null) return;
            var file = checks.Data.Where(f => f.Name == "TestSecret").FirstOrDefault();
            Assert.NotNull(file);
            if (file is not null)
                Assert.True(file.Content == Convert.ToBase64String(Encoding.UTF8.GetBytes("Test")));
            if (file is null) return;
            result = await Consumer.DeleteFile(file.Id);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            var check = await Consumer.GetFile(file.Id);
            Assert.NotNull(check);
            Assert.False(check.Success);
            file = check.Data;
            Assert.Null(file);
        }

        [Fact]
        public async Task AddFile_Unsuccesfull()
        {
            var result = await Consumer.AddFile(new FileDto()
            {
            });
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public async Task DeleteFile_Unsuccesfull()
        {
            var result = await Consumer.DeleteFile(82200);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }

    }
}
