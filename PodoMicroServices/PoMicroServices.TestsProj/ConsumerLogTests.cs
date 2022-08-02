using PodoMicroServices.Common;
using PodoMicroServices.Common.Dto.FileDto;
using PodoMicroServices.Common.Dto.LogDto;
using PodoMicroServices.Consumer;
using System.Text;

namespace PoMicroServices.TestsProj
{
    public class ConsumerLogTests
    {
        public PodoConsumer Consumer { get; set; }

        public ConsumerLogTests()
        {
            Consumer = new PodoConsumer("https://localhost:1312");
            var result = Consumer.Login("TestUser", "TestUser", 3).Result;
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetLogs()
        {
            var result = await Consumer.GetLogs();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);

        }

        [Fact]
        public async Task GetLogsPerGroupSession_Succesful()
        {
            var result = await Consumer.GetLogs();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            var desiredLog = result.Data.FirstOrDefault();
            if (desiredLog != null)
            {
                result = await Consumer.GetLogs(desiredLog.GroupSession);
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.True(string.IsNullOrEmpty(result.Message));
                Assert.NotNull(result.Data);
                if (result.Data is null) return;
                Assert.Null(result.Data.Where(s => s.GroupSession != desiredLog.GroupSession).FirstOrDefault());
            }
            
        }

        [Fact]
        public async Task GetLogsPerSeverity()
        {
            
            var result = await Consumer.GetLogs(Severity.Trace);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            Assert.Null(result.Data.Where(s => s.Severity != Severity.Trace).FirstOrDefault());

            result = await Consumer.GetLogs(Severity.Information);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            Assert.Null(result.Data.Where(s => s.Severity != Severity.Information).FirstOrDefault());

            result = await Consumer.GetLogs(Severity.Debug);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            Assert.Null(result.Data.Where(s => s.Severity != Severity.Debug).FirstOrDefault());

            result = await Consumer.GetLogs(Severity.Fatal);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            Assert.Null(result.Data.Where(s => s.Severity != Severity.Fatal).FirstOrDefault());

            result = await Consumer.GetLogs(Severity.Warning);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            Assert.Null(result.Data.Where(s => s.Severity != Severity.Warning).FirstOrDefault());

            result = await Consumer.GetLogs(Severity.Error);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            Assert.NotNull(result.Data);
            if (result.Data is null) return;
            Assert.Null(result.Data.Where(s => s.Severity != Severity.Error).FirstOrDefault());
        }

        [Fact]
        public async Task FullCircleSecret()
        {
            var result = await Consumer.WriteLog(new LogDto()
            {

                Name = "TestSecret",
                Message = "Test",
                GroupSession = Guid.NewGuid(),
                Severity = Severity.Information,
                Created = DateTime.Now
            });
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            var checks = await Consumer.GetLogs();
            Assert.NotNull(checks);
            Assert.True(checks.Success);
            Assert.NotNull(checks.Data);
            if (checks.Data is null) return;
            var log = checks.Data.Where(f => f.Message == "Test").FirstOrDefault();
            Assert.NotNull(log);
            if (log is not null)
                Assert.True(log.Name == "TestSecret");
            if (log is null) return;
            result = await Consumer.DeleteLog(log.Id);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Message));
            checks = await Consumer.GetLogs();
            Assert.NotNull(checks);
            Assert.True(checks.Success);
            Assert.NotNull(checks.Data);
            if (checks.Data is null) return;
            log = checks.Data.Where(f => f.Id == log.Id).FirstOrDefault();
            Assert.Null(log);
        }

        [Fact]
        public async Task AddLog_Unsuccesfull()
        {
            var result = await Consumer.WriteLog(new LogDto()
            {
            });
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public async Task DeleteLog_Unsuccesfull()
        {
            var result = await Consumer.DeleteLog(int.MaxValue);
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }

    }
}
